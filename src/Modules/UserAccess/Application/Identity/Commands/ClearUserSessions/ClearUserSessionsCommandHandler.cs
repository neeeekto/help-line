using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Application.Identity.Services;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Identity.Commands.ClearUserSessions
{
    class ClearUserSessionsCommandHandler : ICommandHandler<ClearUserSessionsCommand>
    {

        private readonly SessionManager _sessionManager;


        public ClearUserSessionsCommandHandler(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task<Unit> Handle(ClearUserSessionsCommand request, CancellationToken cancellationToken)
        {
            await _sessionManager.ClearAll(request.UserId);
            return Unit.Value;
        }
    }
}
