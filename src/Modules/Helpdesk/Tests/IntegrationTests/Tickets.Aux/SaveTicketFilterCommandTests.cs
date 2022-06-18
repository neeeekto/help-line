using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTicketFilter;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketFilters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class SaveTicketFilterCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(SaveTicketFilterCommandTests);

        private const string TEST_FEAT = "test";
        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
            await CreateOperator();
        }

        private TicketSavedFilterData MakeData() => new TicketSavedFilterData
        {
            Name = TestStr,
            Filter = new TicketFilterGroup(),
            Features = new [] {TEST_FEAT},
            Order = 1
        };

        [Test]
        public async Task SaveTicketFilterCommand_WhenIsValid_IsSuccessful()
        {
            var data = MakeData();
            var entityId = await Module.ExecuteCommandAsync(new SaveTicketFilterCommand(ProjectId, data));

            var entity = await Module.ExecuteQueryAsync(new GetTicketFilterQuery(entityId));

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.Id, Is.EqualTo(entityId));
            Assert.That(entity.ProjectId, Is.EqualTo(ProjectId));
            Assert.That(entity.Name, Is.EqualTo(data.Name));
            Assert.That(entity.Changed, Is.LessThan(DateTime.UtcNow));
            Assert.That(entity.Features, Does.Contain(TEST_FEAT));
            Assert.That(entity.Filter, Is.TypeOf<TicketFilterGroup>());
            Assert.That(entity.Share, Is.Null);
            Assert.That(entity.Order, Is.EqualTo(data.Order));
        }

        [Test]
        public async Task SaveTicketFilterCommand_CanUpdate_IsSuccessful()
        {
            var data = MakeData();
            var entityId = await Module.ExecuteCommandAsync(new SaveTicketFilterCommand(ProjectId, data));
            data.Name = Guid.NewGuid().ToString();
            var updatedEntityId = await Module.ExecuteCommandAsync(new SaveTicketFilterCommand(ProjectId, data, entityId));
            var entity = await Module.ExecuteQueryAsync(new GetTicketFilterQuery(entityId));

            Assert.That(entity, Is.Not.Null);
            Assert.That(updatedEntityId, Is.EqualTo(entityId));
            Assert.That(entity.Name, Is.EqualTo(data.Name));
        }

        public class InvalidSource : TicketSavedFilterData
        {
            public string ProjectId = HelpdeskTestBase.ProjectId;

            public InvalidSource()
            {
                Name = TestStr;
                Share = null;
                Filter = new TicketFilterGroup();
                Features = new[] {"test"};
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
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Name = null
                        }
                    ).SetName("Invalid name: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Filter = null
                        }
                    ).SetName("Invalid filter: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Features = null
                        }
                    ).SetName("Invalid features: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Share = new TicketFilterShareForOperators() {Operators = new []{Guid.NewGuid()}}
                        }
                    ).SetName("Invalid operators: Not exist");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task SaveTicketFilterCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new SaveTicketFilterCommand(src.ProjectId, src);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
