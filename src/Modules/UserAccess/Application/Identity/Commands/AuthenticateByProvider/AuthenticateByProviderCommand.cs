using System;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Identity.Commands.AuthenticateByProvider
{
    public class AuthenticateByProviderCommand : CommandBase<Guid?>
    {
        public string? Provider { get; }
        public string ProviderUserId { get; }

        public AuthenticateByProviderCommand(string? provider, string providerUserId)
        {
            Provider = provider;
            ProviderUserId = providerUserId;
        }
    }
}
