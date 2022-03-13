using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HelpLine.Apps.Identity.Configuration.Authorization;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Identity.Queries;
using HelpLine.Modules.UserAccess.Application.Identity.Queries.GetUserPermissions;
using HelpLine.Modules.UserAccess.Application.Users.Queries;
using HelpLine.Modules.UserAccess.Application.Users.Queries.GetUsers;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace HelpLine.Apps.Identity.Configuration.Authentication
{
    public class ProfileService : IProfileService
    {
        private readonly IUserAccessModule _userAccessModule;
        private readonly AdminSettings _adminSettings;

        public ProfileService(IUserAccessModule userAccessModule,
            AdminSettings adminSettings)
        {
            _userAccessModule = userAccessModule;
            _adminSettings = adminSettings;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var adminUser = _adminSettings.FindAdmin(sub);
            if (adminUser == null)
            {
                var permissionTask = _userAccessModule.ExecuteQueryAsync(new GetUserPermissionsQuery(Guid.Parse(sub)));
                var userTask = _userAccessModule.ExecuteQueryAsync(new GetUserQuery(Guid.Parse(sub)));

                await Task.WhenAll(
                    userTask,
                    permissionTask
                );
                context.IssuedClaims.AddRange(UserClaimBuilder.Make(permissionTask.Result));
                context.IssuedClaims.AddRange(UserClaimBuilder.Make(userTask.Result));
            }
            else
            {
                context.IssuedClaims.AddRange(AdminClaimBuilder.Make());
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(context.IsActive);
        }
    }
}
