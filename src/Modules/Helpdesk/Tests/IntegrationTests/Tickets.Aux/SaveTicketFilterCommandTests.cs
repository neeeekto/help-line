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
using HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class SaveTicketFilterCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(SaveTicketFilterCommandTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
            await CreateOperator();
        }

        private TicketFilterData MakeData() => new TicketFilterData
        {
            Name = TestStr,
            Filter = new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(TestStr), TestStr),
            Features = new [] {TicketFilterFeatures.Important}
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
            Assert.That(entity.Features, Does.Contain(TicketFilterFeatures.Important));
            Assert.That(entity.Filter, Is.TypeOf<ValueFilter>());
            Assert.That(entity.Share, Is.Null);
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

        [Test]
        public async Task SaveTicketFilterCommand_DefaultWillBeGlobal_IsSuccessful()
        {
            var data = MakeData();
            data.Features = new[] { TicketFilterFeatures.Default };
            var entityId = await Module.ExecuteCommandAsync(new SaveTicketFilterCommand(ProjectId, data));

            var entity = await Module.ExecuteQueryAsync(new GetTicketFilterQuery(entityId));

            Assert.That(entity.Features, Does.Contain(TicketFilterFeatures.Default));
            Assert.That(entity.Share, Is.Not.Null);
            Assert.That(entity.Share, Is.TypeOf<TicketFilterShareGlobal>());
        }

        public class InvalidSource : TicketFilterData
        {
            public string ProjectId = HelpdeskTestBase.ProjectId;

            public InvalidSource()
            {
                Name = TestStr;
                Share = null;
                Filter = new ValueFilter(FieldFilterOperators.Equal, new ConstantFilterValue(TestStr), TestStr);
                Features = new[] {TicketFilterFeatures.Default};
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
