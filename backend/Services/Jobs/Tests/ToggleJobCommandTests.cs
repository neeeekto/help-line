using System;
using System.Threading.Tasks;
using HelpLine.Services.Jobs.Application;
using HelpLine.Services.Jobs.Application.Commands;
using HelpLine.Services.Jobs.Application.Commands.CreateJob;
using HelpLine.Services.Jobs.Application.Commands.ToggleJob;
using HelpLine.Services.Jobs.Application.DTO;
using HelpLine.Services.Jobs.Application.Queries;
using HelpLine.Services.Jobs.Application.Queries.GetJobsTriggerState;
using HelpLine.Services.Jobs.Tests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Services.Jobs.Tests
{
    public class ToggleJobCommandTests : JobTestsBase
    {
        protected override string DbName => nameof(ToggleJobCommandTests);

        [Test]
        public async Task ToggleJobCommand_Enable_WhenDataIsValid_IsSuccessful()
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
            await Service.ExecuteAsync(new ToggleJobCommand(jobId, true));

            var jobsStatus = await Service.ExecuteAsync(new GetJobsTriggerStateQuery(new[] {jobId}));
            var isExist = jobsStatus.TryGetValue(jobId, out var triggerState);
            Assert.That(isExist, Is.True);
            Assert.That(triggerState, Is.Not.Null);
            Assert.That(triggerState.Next, Is.Not.Null);
            Assert.That(triggerState.Next, Is.GreaterThan(DateTime.UtcNow));
        }

        [Test]
        public async Task ToggleJobCommand_Disable_WhenDataIsValid_IsSuccessful()
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
            await Service.ExecuteAsync(new ToggleJobCommand(jobId, true));
            await Service.ExecuteAsync(new ToggleJobCommand(jobId, false));

            var jobsStatus = await Service.ExecuteAsync(new GetJobsTriggerStateQuery(new[] {jobId}));
            var isExist = jobsStatus.TryGetValue(jobId, out var triggerState);
            Assert.That(isExist, Is.True);
            Assert.That(triggerState, Is.Null);
        }
    }
}
