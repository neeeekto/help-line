using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Services.Jobs.Application;
using HelpLine.Services.Jobs.Application.Commands;
using HelpLine.Services.Jobs.Application.Commands.CreateJob;
using HelpLine.Services.Jobs.Application.Commands.ToggleJob;
using HelpLine.Services.Jobs.Application.DTO;
using HelpLine.Services.Jobs.Tests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Services.Jobs.Tests
{
    [TestFixture]
    [NonParallelizable]
    public class FireJobCommandTests : JobTestsBase
    {
        protected override string DbName => nameof(FireJobCommandTests);

        [Test]
        public async Task FireJobCommand_IsSuccessful()
        {
            var jobData = new TestJobDataBase();
            var jobDto = new JobDataDto()
            {
                Data = jobData,
                Group = "test",
                Name = "test",
                Schedule ="0/1 * * * * ?"
            };
            var jobId = await Service.ExecuteAsync(new CreateJobCommand(jobDto, typeof(TestJobTask).FullName));
            await Service.ExecuteAsync(new ToggleJobCommand(jobId, true));
            await Startup.Start();
            await Task.Delay(new TimeSpan(TimeSpan.TicksPerSecond + 500));
            var emited = BusServiceFactory.Queues.Any(x => x.Value.Queue.Any());
            Assert.That(emited, Is.True);
        }
    }
}
