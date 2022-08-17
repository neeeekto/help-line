using System;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.AddScenario;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Macros
{
    public class AddScenarioCommandTests : TicketMacrosTestBase
    {
        protected override string NS => nameof(TicketMacrosTestBase);

        [Test]
        public async Task AddScenarioCommand_WhenIsValid_IsSuccessful()
        {
            var cmd = new AddScenarioCommand(
                TestStr,
                TestStr,
                1,
                new ScenarioTriggerBase[]
                {
                    new TicketCreatedScenarioTrigger()
                },
                new[]
                {
                    new AddTicketNoteAction()
                    {
                        Message = new MessageDto(TestStr)
                    }
                },
                new[] {Guid.NewGuid()},
                ErrorBehavior.Stop);
            var scenario = await Module.ExecuteCommandAsync(cmd);

            Assert.That(scenario.Id, Is.Not.Null);
            Assert.That(scenario.Name, Is.EqualTo(cmd.Name));
            Assert.That(scenario.Description, Is.EqualTo(cmd.Description));
        }
        // TODO: Make other tests!!!
    }
}
