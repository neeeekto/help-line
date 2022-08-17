using System.Globalization;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Services.Jobs.Models;
using MongoDB.Driver;
using Quartz;
using Serilog;

namespace HelpLine.Services.Jobs.QuartzJobs
{
    [DisallowConcurrentExecution]
    internal class SyncJob : IJob
    {
        private const string ModificationKey = "mod";
        private readonly IMongoContext _context;
        private readonly ILogger _logger;

        public SyncJob(IMongoContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobsTasksCollection = _context.GetCollection<Job>();
            var jobsTasks = await jobsTasksCollection.Find(x => true).ToListAsync();
            foreach (var job in jobsTasks)
            {
                await CheckAndUpdateJob(job, context);
            }
        }

        private async Task CheckAndUpdateJob(Job job, IJobExecutionContext context)
        {
            var triggerKey = new TriggerKey(job.Id.ToString());
            var currentTrigger = await context.Scheduler.GetTrigger(triggerKey);
            var currentTriggerState = await context.Scheduler.GetTriggerState(triggerKey);
            var jobLogName = $"{job.Id}/{job.Name}[{job.Schedule}]";
            switch (currentTriggerState)
            {
                case TriggerState.None when job.Enabled:
                    await JobManager.Create(job, context.Scheduler);
                    _logger.Information($"Job {jobLogName} planned");
                    break;

                case TriggerState.Blocked when !job.Enabled: //state "Blocked" means, that job is executed right now and trigger tick is added to queue
                case TriggerState.Normal when !job.Enabled:
                    await JobManager.Cancel(job, context.Scheduler);
                    _logger.Information($"Job {jobLogName} removed");
                    break;

                // Update a job, if setting is changing
                case TriggerState.Normal when job.Enabled:
                    if (currentTrigger.JobDataMap.GetString(ModificationKey) !=
                        job.ModificationDate.ToString(CultureInfo.InvariantCulture))
                    {
                        await JobManager.Cancel(job, context.Scheduler);
                        await JobManager.Create(job, context.Scheduler);
                        _logger.Information($"Job {jobLogName} updated");
                    }
                    break;
            }
        }
    }
}
