using System;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Identity.Commands;
using HelpLine.Modules.UserAccess.Application.Identity.Commands.AuthenticateByPassword;
using HelpLine.Modules.UserAccess.Application.Identity.Queries;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Serilog;

namespace HelpLine.Apps.Identity.Configuration.Authorization
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserAccessModule _userAccessModule;
        private readonly ILogger _logger;

        public ResourceOwnerPasswordValidator(IUserAccessModule userAccessModule, ILogger logger)
        {
            _userAccessModule = userAccessModule;
            _logger = logger;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var userId =
                    await _userAccessModule.ExecuteCommandAsync(
                        new AuthenticateByPasswordCommand(context.UserName, context.Password));
                if (userId != null)
                {
                    context.Result = new GrantValidationResult(
                        userId.ToString(),
                        "forms"
                    );
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.Error("Password validation exception", e);
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant,
                    e.Message);
                return;
            }

            context.Result = new GrantValidationResult(
                TokenRequestErrors.InvalidGrant,
                "User not found by email/password");
        }
    }
}
