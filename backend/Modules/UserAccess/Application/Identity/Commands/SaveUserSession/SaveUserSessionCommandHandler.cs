using System;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Application.Identity.Services;

namespace HelpLine.Modules.UserAccess.Application.Identity.Commands.SaveUserSession
{
    class SaveUserSessionCommandHandler : ICommandHandler<SaveUserSessionCommand, Guid>
    {
        private readonly SessionManager _sessionManager;

        public SaveUserSessionCommandHandler(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }


        public async Task<Guid> Handle(SaveUserSessionCommand request, CancellationToken cancellationToken)
        {
            var session = new UserSession(Guid.NewGuid(), request.UserId, request.Data, DateTime.UtcNow,
                DateTime.UtcNow + request.Lifetime
            );
            await _sessionManager.Save(session, request.Lifetime);
            return session.SessionId;
        }
    }
}
