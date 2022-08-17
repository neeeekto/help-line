using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HelpLine.Apps.Client.API.Configuration.Middlewares;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.UserAccess.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace HelpLine.Apps.Client.API.Configuration.Authorization
{
    internal class HasPermissionAuthorizationHandler : AttributeAuthorizationHandler<
        HasPermissionAuthorizationRequirement, HasPermissionsAttribute>
    {
        private readonly IUserAccessModule _userAccessModule;
        private readonly IExecutionContextAccessor _executionContextAccessor;

        public HasPermissionAuthorizationHandler(
            IExecutionContextAccessor executionContextAccessor,
            IUserAccessModule userAccessModule)
        {
            _executionContextAccessor = executionContextAccessor;
            _userAccessModule = userAccessModule;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            HasPermissionAuthorizationRequirement requirement, IEnumerable<HasPermissionsAttribute> attributes)
        {
            var project =
                (context.Resource as DefaultHttpContext)?.HttpContext.Request.Headers[
                    ProjectMiddleware.ProjectHeaderKey];

            foreach (var hasPermissionsAttribute in attributes)
            {
                if (!Authorize(project, hasPermissionsAttribute.Permissions, hasPermissionsAttribute.Inclusion,
                    context.User.Claims))
                {
                    context.Fail();
                    return;
                }
            }

            context.Succeed(requirement);
        }

        private bool Authorize(string? project, IEnumerable<string>? needPermissions,
            HasPermissionsAttribute.InclusionType inclusionType, IEnumerable<Claim> userClaims)
        {
            if (userClaims.Any(x => x.Type == "isAdmin") || needPermissions?.Any() == false)
                return true;

            var gamePermissions = userClaims.Where(x => x.Type.Contains($"{project}.")).Select(x => x.Value);
            var commonPermissions = userClaims.Where(x => x.Type.Contains("permission")).Select(x => x.Value);
            var userPermissions = !string.IsNullOrEmpty(project) && gamePermissions.Any()
                ? gamePermissions
                : commonPermissions;
            if (inclusionType == HasPermissionsAttribute.InclusionType.And)
            {
                return needPermissions.All(x => userPermissions.Contains(x));
            }

            return needPermissions.Any(x => userPermissions.Contains(x));
        }
    }
}
