using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Services.Jobs.Application;
using HelpLine.Services.Jobs.Application.Commands;
using HelpLine.Services.Jobs.Application.Commands.CreateJob;
using HelpLine.Services.Jobs.Application.Commands.DeleteJob;
using HelpLine.Services.Jobs.Application.Commands.ToggleJob;
using HelpLine.Services.Jobs.Application.DTO;
using HelpLine.Services.Jobs.Application.Queries;
using HelpLine.Services.Jobs.Application.Queries.GetJobs;
using HelpLine.Services.Jobs.Tests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Services.Jobs.Tests
{
    [NonParallelizable]
    public class DeleteJobCommandTests : JobTestsBase
    {
        protected override string DbName => nameof(DeleteJobCommandTests);

        [Test]
        public async Task DeleteJobCommand_WhenDisabled_IsSuccessful()
        {
            var jobData = new TestJobDataBase();
            var jobDto = new JobDataDto()
            {
                Data = jobData,
                Group = "test",
                Name = "test",
                Schedule = "* * 5 * * ?"
            };
            var jobId = await Service.ExecuteAsync(new CreateJobCommand(jobDto, typeof(TestJobTask).FullName));
            await Service.ExecuteAsync(new DeleteJobCommand(jobId));

            var jobs = await Service.ExecuteAsync(new GetJobsQuery());
            Assert.That(jobs.Any(), Is.False);
        }

        [Test]
        public async Task DeleteJobCommand_WhenEnabled_IsSuccessful()
        {
            await Startup.Start();
            var jobData = new TestJobDataBase();
            var jobDto = new JobDataDto()
            {
                Data = jobData,
                Group = "test",
                Name = "test",
                Schedule = "* * 5 * * ?"
            };
            var jobId = await Service.ExecuteAsync(new CreateJobCommand(jobDto, typeof(TestJobTask).FullName));
            await Service.ExecuteAsync(new ToggleJobCommand(jobId, true));
            await Service.ExecuteAsync(new DeleteJobCommand(jobId));

            var jobs = await Service.ExecuteAsync(new GetJobsQuery());
            Assert.That(jobs.Any(), Is.False);
            await Startup.Stop();
        }
    }
}
