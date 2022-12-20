using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Identity.Queries.GetUserPermissions;
using IdentityServer4.Validation;

namespace HelpLine.Apps.Identity.Configuration
{
    public class AdminRequestValidator : ICustomAuthorizeRequestValidator
    {
        private readonly AdminSettings _adminSettings;


        public AdminRequestValidator(AdminSettings adminSettings)
        {
            _adminSettings = adminSettings;
        }

        public async Task ValidateAsync(CustomAuthorizeRequestValidationContext context)
        {
            var sub = context.Result.ValidatedRequest.Subject.FindFirst("sub");
            if (sub != null)
            {
                var admin = _adminSettings.FindAdmin(sub.Value);
                var client = context.Result.ValidatedRequest.ClientId;
                var isAdminClient = _adminSettings.IsAdminClient(client);
                if (isAdminClient && admin == null)
                {
                    context.Result.IsError = true;
                    context.Result.Error = "No access";
                    context.Result.ErrorDescription = "You are not authorized for this app";
                }
            }
        }
    }
}
