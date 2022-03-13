using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTicketReopenCondition;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SwitchTicketReopenCondition;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.ToggleTicketReopenCondition;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketReopenConditions;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class SwitchTicketReopenConditionCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(SwitchTicketReopenConditionCommandTests);

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
        public async Task SwitchTicketReopenConditionCommand_WhenIsValid_IsSuccessful()
        {
            var entityId1 = await Module.ExecuteCommandAsync(new SaveTicketReopenConditionCommand(ProjectId, MakeData("test1")));
            var entityId2 = await Module.ExecuteCommandAsync(new SaveTicketReopenConditionCommand(ProjectId, MakeData("test2")));
            await Module.ExecuteCommandAsync(new ToggleTicketReopenConditionCommand(entityId1, true));
            await Module.ExecuteCommandAsync(new SwitchTicketReopenConditionCommand(entityId1, entityId2));

            var entities = await Module.ExecuteQueryAsync(new GetTicketReopenConditionsQuery(ProjectId));
            var entity1 = entities.FirstOrDefault(x => x.Id == entityId1);
            var entity2 = entities.FirstOrDefault(x => x.Id == entityId2);

            Assert.That(entity1.Enabled, Is.False);
            Assert.That(entity2.Enabled, Is.True);
        }
    }
}
