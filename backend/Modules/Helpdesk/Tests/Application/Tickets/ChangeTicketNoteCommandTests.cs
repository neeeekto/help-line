using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
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
    public class ChangeTicketNoteCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(ChangeTicketNoteCommandTests);

        [Test]
        public async Task ChangeTicketNoteCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);

            var noteId =
                await ExecuteAction(ticketId, new AddTicketNoteAction(testData.Message), new SystemInitiatorDto());

            var message = new MessageDto
            {
                Text = "new test"
            };
            var cmd = new ChangeTicketNoteAction(((TicketNoteId) noteId).Value, message);
            await ExecuteAction(ticketId, cmd, new SystemInitiatorDto());

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketNoteEventView>().SingleOrDefault();

            Assert.IsNotNull(evt);
            Assert.IsNotNull(evt.CreateDate);
            Assert.AreEqual(message.Text, evt.Message.Text);
            Assert.AreEqual(1, evt.History.Count());
            Assert.IsNull(evt.DeleteTime);

            var historyItem = evt.History.FirstOrDefault();
            Assert.AreEqual(testData.Message.Text, historyItem.Message.Text);
            Assert.IsInstanceOf<SystemInitiatorView>(historyItem.Initiator);
            Assert.IsTrue(historyItem.Date < DateTime.UtcNow);
        }
    }
}
