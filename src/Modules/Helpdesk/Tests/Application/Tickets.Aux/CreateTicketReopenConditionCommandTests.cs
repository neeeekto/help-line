using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTicketReopenCondition;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketReopenConditions;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class CreateTicketReopenConditionCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(CreateTicketReopenConditionCommandTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        private TicketReopenConditionData MakeData() => new TicketReopenConditionData
        {
            Name = TestStr,
            MinimalScore = 4,
            MustSolved = true
        };

        [Test]
        public async Task CreateTicketReopenConditionCommand_WhenIsValid_IsSuccessful()
        {
            var data = MakeData();
            var entityId = await Module.ExecuteCommandAsync(new SaveTicketReopenConditionCommand(ProjectId, data));

            var entities = await Module.ExecuteQueryAsync(new GetTicketReopenConditionsQuery(ProjectId));
            var entity = entities.FirstOrDefault(x => x.Id == entityId);

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.Id, Is.EqualTo(entityId));
            Assert.That(entity.Enabled, Is.False);
            Assert.That(entity.ProjectId, Is.EqualTo(ProjectId));
            Assert.That(entity.MinimalScore, Is.EqualTo(data.MinimalScore));
            Assert.That(entity.Name, Is.EqualTo(data.Name));
            Assert.That(entity.MustSolved, Is.EqualTo(data.MustSolved));
        }

        public class InvalidSource : TicketReopenConditionData
        {
            public string ProjectId = HelpdeskTestBase.ProjectId;

            public InvalidSource()
            {
                Name = TestStr;
                MinimalScore = 4;
                MustSolved = false;
            }

            public static IEnumerable<TestCaseData> Cases
            {
                get
                {
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = ""
                        }
                    ).SetName("Empty project: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = null
                        }
                    ).SetName("Empty project: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = "asd"
                        }
                    ).SetName("Not exist project");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Name = ""
                        }
                    ).SetName("Invalid name: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Name = null
                        }
                    ).SetName("Invalid name: Null");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task CreateTicketReopenConditionCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new SaveTicketReopenConditionCommand(src.ProjectId, src);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
