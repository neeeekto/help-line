using System;
using System.Globalization;
using System.Threading.Tasks;
using HelpLine.Services.Jobs.Models;
using HelpLine.Services.Jobs.QuartzJobs;
using Newtonsoft.Json;
using Quartz;

namespace HelpLine.Services.Jobs
{
    internal class JobManager
    {
        private const string ModificationKey = "mod";

        public static async Task Cancel(Guid jobId, IScheduler scheduler)
        {
            var triggerKey = new TriggerKey(jobId.ToString());
            await scheduler.Interrupt(new JobKey(jobId.ToString()));
            await scheduler.UnscheduleJob(triggerKey);
        }

        public static Task Cancel(Job job, IScheduler scheduler)
        {
            return Cancel(job.Id, scheduler);
        }

        public static async Task Create(Job job, IScheduler scheduler)
        {
            var triggerKey = new TriggerKey(job.Id.ToString());
            var jobInstance = JobBuilder.Create<ExecutorJob>()
                .UsingJobData("job", JsonConvert.SerializeObject(job, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }))
                .WithIdentity(job.Id.ToString())
                .Build();
            var jobTrigger = TriggerBuilder
                .Create()
                .StartNow()
                .WithIdentity(triggerKey)
                .UsingJobData(ModificationKey, job.ModificationDate.ToString(CultureInfo.InvariantCulture))
                .WithCronSchedule(job.Schedule)
                .Build();
            await scheduler.ScheduleJob(jobInstance, jobTrigger);
        }
    }
}
