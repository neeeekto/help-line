using System.Threading;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Configuration.Jobs;
using HelpLine.Modules.UserAccess.Application.Identity.Services;
using HelpLine.Modules.UserAccess.Jobs;
using MediatR;

namespace HelpLine.Modules.UserAccess.Application.Jobs
{
    public class SyncSessionsJobHandler: IJobHandler<SyncSessionsJob>
    {
        private readonly SessionManager _sessionManager;

        public SyncSessionsJobHandler(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task<Unit> Handle(SyncSessionsJob request, CancellationToken cancellationToken)
        {
            await _sessionManager.Sync();
            return Unit.Value;
        }
    }
}
