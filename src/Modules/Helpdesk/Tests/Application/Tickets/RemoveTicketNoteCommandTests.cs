using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    public class RemoveTicketNoteCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(RemoveTicketNoteCommandTests);

        [Test]
        public async Task RemoveTicketNoteCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);

            var noteId = await ExecuteAction(ticketId, new AddTicketNoteAction(
                new MessageDto {Text = "test"}), new SystemInitiatorDto());

            await ExecuteAction(ticketId, new RemoveTicketNoteAction(((TicketNoteId) noteId).Value),
                new SystemInitiatorDto());
            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketNoteEventView>().FirstOrDefault();

            Assert.IsNotNull(evt);
            Assert.IsNotNull(evt.DeleteTime);
            Assert.IsTrue(evt.DeleteTime <= DateTime.UtcNow);
        }
    }
}
