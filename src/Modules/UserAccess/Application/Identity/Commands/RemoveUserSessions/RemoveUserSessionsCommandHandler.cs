using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Application.Identity.Services;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Identity.Commands.RemoveUserSessions
{
    class RemoveUserSessionsCommandHandler : ICommandHandler<RemoveUserSessionsCommand>, ICommandHandler<RemoveUserSessionCommand>
    {
        private readonly SessionManager _sessionManager;

        public RemoveUserSessionsCommandHandler(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task<Unit> Handle(RemoveUserSessionsCommand request, CancellationToken cancellationToken)
        {
            await _sessionManager.ClearAll(request.UserId);
            return Unit.Value;
        }

        public async Task<Unit> Handle(RemoveUserSessionCommand request, CancellationToken cancellationToken)
        {
            await _sessionManager.Clear(request.SessionId);
            return Unit.Value;
        }
    }
}
