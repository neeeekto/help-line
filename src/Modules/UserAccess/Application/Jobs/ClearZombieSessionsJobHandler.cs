using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Configuration.Jobs;
using HelpLine.Modules.UserAccess.Application.Identity.Services;
using HelpLine.Modules.UserAccess.Jobs;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Jobs
{
    internal class ClearZombieSessionsJobHandler : IJobHandler<ClearZombieSessionsJob>
    {
        private readonly SessionManager _sessionManager;

        public ClearZombieSessionsJobHandler(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task<Unit> Handle(ClearZombieSessionsJob request, CancellationToken cancellationToken)
        {
            var sessions = await _sessionManager.GetList();
            var expiredSessions = sessions.Where(x => x.Expired < DateTime.UtcNow);
            await _sessionManager.Clear(expiredSessions.Select(x => x.SessionId).ToArray());
            return Unit.Value;
        }
    }
}
