using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.ExecuteTicketAction;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    [TestFixture]
    public class AddTicketReminderCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(AddTicketNoteCommandTests);

        [Test]
        public async Task AddTicketReminderCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            var message = new MessageDto {Text = "out"};
            var reminder = new TicketFinalReminderDto
            {
                Delay = TimeSpan.FromMinutes(2),
                Message = message,
                Resolve = false
            };
            var cmd = new AddTicketReminderAction(reminder);
            var reminderId = await Module.ExecuteCommandAsync(new ExecuteTicketActionCommand(ticketId, cmd,  new SystemInitiatorDto()));

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var reminderEvt = ticketView.Events.OfType<TicketReminderEventView>().FirstOrDefault();

            Assert.IsNotNull(reminderEvt);
            Assert.IsNotNull(reminderEvt.CreateDate);
            Assert.IsNull(reminderEvt.Result);
            Assert.IsNull(reminderEvt.Reminder.Next);
            Assert.AreEqual(reminderEvt.Reminder.Id, ((TicketReminderId)reminderId).Value);
            Assert.AreEqual(reminderEvt.Reminder.Message.Text, reminder.Message.Text);
            Assert.AreEqual(reminderEvt.Reminder.Resolving, reminder.Resolve);
            Assert.IsTrue(reminderEvt.Reminder.SendDate > DateTime.UtcNow);
            Assert.IsTrue(reminderEvt.Reminder.SendDate - DateTime.UtcNow > TimeSpan.FromSeconds(115));
            Assert.IsTrue(reminderEvt.Reminder.SendDate - DateTime.UtcNow <= TimeSpan.FromSeconds(120));
        }
    }
}
