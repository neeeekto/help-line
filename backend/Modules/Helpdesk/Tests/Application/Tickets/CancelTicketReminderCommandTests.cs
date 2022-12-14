using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.ExecuteTicketAction;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    [TestFixture]
    public class CancelTicketReminderCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(CancelTicketRejectCommandTests);

        [Test]
        public async Task CancelTicketReminderCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);

            var reminderId = await Module.ExecuteCommandAsync(
                new ExecuteTicketActionCommand(ticketId,
                    new AddTicketReminderAction(new TicketFinalReminderDto
                    {
                        Delay = TimeSpan.FromDays(10),
                        Message = new MessageDto
                        {
                            Text = "Test"
                        },
                        Resolve = true
                    }),
                    new SystemInitiatorDto()
                )
            );


            var cmd = new CancelTicketReminderAction(((TicketReminderId) reminderId).Value);
            await Module.ExecuteCommandAsync(new ExecuteTicketActionCommand(ticketId, cmd, new SystemInitiatorDto()));

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketReminderEventView>().FirstOrDefault();

            Assert.IsNotNull(evt);
            Assert.IsNotNull(evt.CreateDate);
            Assert.IsNotNull(evt.Result);
            Assert.IsInstanceOf<SystemInitiatorView>(evt.Result.Initiator);
            Assert.IsNotNull(evt.Result.Date);
            Assert.IsTrue(evt.Result.Date < DateTime.UtcNow);
            Assert.IsInstanceOf<ScheduledEventCanceledResultView>(evt.Result);
        }
    }
}
