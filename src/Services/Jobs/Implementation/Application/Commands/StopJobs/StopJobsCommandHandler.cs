using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Quartz;

namespace HelpLine.Services.Jobs.Application.Commands.StopJobs
{
    internal class StopJobsCommandHandler : IRequestHandler<StopJobsCommand>
    {
        private readonly IScheduler _scheduler;

        public StopJobsCommandHandler(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public async Task<Unit> Handle(StopJobsCommand request, CancellationToken cancellationToken)
        {
            await _scheduler.Shutdown(cancellationToken);
            return Unit.Value;
        }
    }
}
