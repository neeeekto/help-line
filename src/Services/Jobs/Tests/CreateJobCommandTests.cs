using System;
using System.Threading.Tasks;
using HelpLine.Services.Jobs.Application.Commands.CreateJob;
using HelpLine.Services.Jobs.Application.DTO;
using HelpLine.Services.Jobs.Application.Queries.GetJobs;
using HelpLine.Services.Jobs.Tests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Services.Jobs.Tests
{
    public class CreateJobCommandTests : JobTestsBase
    {
        protected override string DbName => nameof(CreateJobCommandTests);

        [Test]
        public async Task CreateJobCommand_WhenDataIsValid_IsSuccessful()
        {
            var jobData = new TestJobDataBase();
            var jobDto = new JobDataDto()
            {
                Data = jobData,
                Group = "test",
                Name = "test",
                Schedule = "* * * * *"
            };
            var cmd = new CreateJobCommand(jobDto, typeof(TestJobTask).FullName);

            var jobId = await Service.ExecuteAsync(cmd);
            var job = await Service.ExecuteAsync(new GetJobQuery(jobId));
            Assert.That(job, Is.Not.Null);
            Assert.That(job.Enabled, Is.False);
            Assert.That(job.Group, Is.EqualTo(jobDto.Group));
            Assert.That(job.Name, Is.EqualTo(jobDto.Name));
            Assert.That(job.Id, Is.Not.Null);
            Assert.That(job.Schedule, Is.EqualTo(jobDto.Schedule));
            Assert.That(job.TaskType, Is.EqualTo(cmd.Task));
            Assert.That(job.ModificationDate, Is.LessThan(DateTime.UtcNow));

            var savedData = (TestJobDataBase) job.Data;
            Assert.That(jobData.Test, Is.EqualTo(savedData.Test));
            Assert.That(jobData.TestData.Test, Is.EqualTo(savedData.TestData.Test));

        }
    }
}
