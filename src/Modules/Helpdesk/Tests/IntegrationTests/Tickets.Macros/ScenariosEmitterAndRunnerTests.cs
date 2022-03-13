using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.AddScenario;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Commands.ToggleScenario;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Macros
{
    public class ScenariosEmitterAndRunnerTests : TicketMacrosTestBase
    {
        protected override string NS => nameof(ScenariosEmitterAndRunnerTests);

        [Test]
        public async Task RunWithValidTrigger_Expect_Success()
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
                    new AddTicketNoteAction
                    {
                        Message = new MessageDto(TestStr)
                    }
                },
                new[] {Guid.NewGuid()},
                ErrorBehavior.Stop);
            var scenario = await Module.ExecuteCommandAsync(cmd);
            await Module.ExecuteCommandAsync(new ToggleScenarioCommand(scenario.Id, true));

            await CreateProject();
            await CreateOperator();
            var ticketId = await CreateTicket(new TicketTestData());
            await BusServiceFactory.EmitAll();
            var ticket = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));

            var noteEvt = ticket.Events.OfType<TicketNoteEventView>().FirstOrDefault();

            Assert.That(noteEvt, Is.Not.Null);
            Assert.That(noteEvt.Message.Text, Is.EqualTo(TestStr));
            Assert.That(noteEvt.Initiator, Is.TypeOf<SystemInitiatorView>());
        }
    }
}
