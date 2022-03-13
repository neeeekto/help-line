using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTicketReopenCondition;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.ToggleTicketReopenCondition;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketReopenConditions;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class ToggleTicketReopenConditionCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(ToggleTicketReopenConditionCommandTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        private TicketReopenConditionData MakeData(string? name = null) => new TicketReopenConditionData
        {
            Name = name ?? TestStr,
            MinimalScore = 4,
            MustSolved = true
        };

        [Test]
        public async Task ToggleTicketReopenConditionCommand_WhenIsValid_IsSuccessful()
        {
            var entityId =
                await Module.ExecuteCommandAsync(new SaveTicketReopenConditionCommand(ProjectId, MakeData("test1")));
            await Module.ExecuteCommandAsync(new ToggleTicketReopenConditionCommand(entityId, true));
            var entities = await Module.ExecuteQueryAsync(new GetTicketReopenConditionsQuery(ProjectId));
            var entity = entities.FirstOrDefault(x => x.Id == entityId);

            Assert.That(entity.Enabled, Is.True);
        }

        [Test]
        public async Task ToggleTicketReopenConditionCommand_WhenExistEnabled_ThrowInvalidCommandException()
        {
            var entityId =
                await Module.ExecuteCommandAsync(new SaveTicketReopenConditionCommand(ProjectId, MakeData("test1")));
            await Module.ExecuteCommandAsync(new ToggleTicketReopenConditionCommand(entityId, true));

            var entityId2 =
                await Module.ExecuteCommandAsync(new SaveTicketReopenConditionCommand(ProjectId, MakeData("test2")));

            Assert.CatchAsync<InvalidCommandException>(() =>
                Module.ExecuteCommandAsync(new ToggleTicketReopenConditionCommand(entityId2, true)));
        }
    }
}
