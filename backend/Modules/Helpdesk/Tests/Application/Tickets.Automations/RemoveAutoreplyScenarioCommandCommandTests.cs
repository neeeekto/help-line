using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands.RemoveAutoreplyScenario;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands.SaveAutoreplyScenario;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Queries.GetAutoreplyScenarios;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Automations
{
    [NonParallelizable]
    [TestFixture]
    public class RemoveAutoreplyScenarioCommandTests : TicketAutomationsTestBase
    {
        protected override string NS => nameof(RemoveAutoreplyScenarioCommandTests);


        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
            await CreateOperator();
        }

        public AutoreplyScenarioAction MakeAction() => new AutoreplyScenarioAction
        {
            Message = new LocalizeDictionary<MessageDto>()
            {
                {
                    "en", new MessageDto()
                    {
                        Text = "test"
                    }
                }
            },
            Reminder = null,
            Resolve = false,
            Tags = new[] {Tag}
        };

        public AutoreplyScenarioCondition MakeCondition(TagCondition condition) => new AutoreplyScenarioCondition
        {
            Attachments = null,
            Keywords = null,
            Languages = new[] {"en"},
            TagConditions = new[] {condition}
        };

        public AutoreplyScenario MakeScenario(AutoreplyScenarioCondition condition, AutoreplyScenarioAction action) =>
            new AutoreplyScenario
            {
                Name = "test",
                Enabled = true,
                Weight = 1,
                ProjectId = ProjectId,
                Condition = condition,
                Action = action
            };

        [Test]
        public async Task RemoveAutoreplyScenarioCommand_WhenDataIsValid_IsSuccessful()
        {
            var tagCond = MakeTagCondition();
            var data = MakeScenario(MakeCondition(tagCond), MakeAction());
            var scenario = await Module.ExecuteCommandAsync(new SaveAutoreplyScenarioCommand(data));
            await Module.ExecuteCommandAsync(new RemoveAutoreplyScenarioCommand(scenario));
            var entities = await Module.ExecuteQueryAsync(new GetAutoreplyScenariosQuery(ProjectId));
            Assert.That(entities, Is.Empty);
        }

        [Test]
        public async Task RemoveAutoreplyScenarioCommand_WhenIsNotExist_IsSuccessful()
        {
            var tagCond = MakeTagCondition();
            var data = MakeScenario(MakeCondition(tagCond), MakeAction());
            await Module.ExecuteCommandAsync(new SaveAutoreplyScenarioCommand(data));

            await Module.ExecuteCommandAsync(new RemoveAutoreplyScenarioCommand("test"));
            var entities = await Module.ExecuteQueryAsync(new GetAutoreplyScenariosQuery(ProjectId));
            Assert.That(entities, Is.Not.Empty);
        }
    }
}
