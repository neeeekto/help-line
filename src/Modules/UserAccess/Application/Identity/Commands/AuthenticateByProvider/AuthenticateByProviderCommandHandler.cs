using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;

namespace HelpLine.Modules.UserAccess.Application.Identity.Commands.AuthenticateByProvider
{
    class AuthenticateByProviderCommandHandler : ICommandHandler<AuthenticateByProviderCommand, Guid?>
    {
        public Task<Guid?> Handle(AuthenticateByProviderCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
