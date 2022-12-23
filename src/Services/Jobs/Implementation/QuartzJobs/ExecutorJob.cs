using System.Linq;
using System.Threading.Tasks;
using HelpLine.Services.Jobs.Application;
using HelpLine.Services.Jobs.Application.Commands;
using HelpLine.Services.Jobs.Application.Commands.FireJob;
using HelpLine.Services.Jobs.Application.Contracts;
using HelpLine.Services.Jobs.Contracts;
using HelpLine.Services.Jobs.Models;
using Newtonsoft.Json;
using Quartz;
using Serilog;

namespace HelpLine.Services.Jobs.QuartzJobs
{
    [DisallowConcurrentExecution]
    internal class ExecutorJob : IJob
    {
        private readonly ILogger _logger;

        public ExecutorJob(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobData = context.JobDetail.JobDataMap.GetString("job");
            var job = JsonConvert.DeserializeObject<Job>(jobData);
            if (job == null)
            {
                _logger.Error(
                    "Job with trigger {key} is not correct. There is error when parsing job data to json: {jobData}",
                    context.Trigger.Key.Name,
                    jobData
                );
                return;
            }

            await JobsService.InternalExecuteAsync(new FireJobCommand(job));
        }
    }
}
