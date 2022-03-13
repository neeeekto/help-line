using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.Jobs.QuartzJobs;
using MediatR;
using Quartz;

namespace HelpLine.Services.Jobs.Application.Commands.RunJobs
{
    internal class RunJobsCommandHandler : IRequestHandler<RunJobsCommand>
    {
        private readonly IScheduler _scheduler;

        public RunJobsCommandHandler(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public async Task<Unit> Handle(RunJobsCommand request, CancellationToken cancellationToken)
        {
            var triggerKey = new TriggerKey(nameof(SyncJob));
            var jobKey = new JobKey(nameof(SyncJob));
            var jobInstance = JobBuilder.Create<SyncJob>()
                .WithIdentity(jobKey)
                .Build();
            var jobTrigger = TriggerBuilder
                .Create()
                .StartNow()
                .WithIdentity(triggerKey)
                .WithCronSchedule("0 0/5 * * * ?")
                .Build();
            await _scheduler.ScheduleJob(jobInstance, jobTrigger, cancellationToken);
            await _scheduler.TriggerJob(jobKey, cancellationToken);
            await _scheduler.Start(cancellationToken);
            return Unit.Value;
        }
    }
}
