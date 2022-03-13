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
using HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets
{
    public class AddTicketNoteCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(AddTicketNoteCommandTests);

        [Test]
        public async Task AddTicketNoteCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            var message = new MessageDto {Text = "out"};
            var cmd = new AddTicketNoteAction()
            {
                Message = message,
                Tags = Array.Empty<string>()
            };
            await Module.ExecuteCommandAsync(new ExecuteTicketActionsCommand(ticketId,
                new SystemInitiatorDto(), cmd));

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var noteEvt = ticketView.Events.OfType<TicketNoteEventView>().FirstOrDefault();

            Assert.That(noteEvt.Message.Text, Is.EqualTo(message.Text));
            Assert.That(noteEvt.DeleteTime, Is.EqualTo(null));
            Assert.That(noteEvt.NoteId, Is.Not.Null);
            Assert.That(noteEvt.Tags, Is.Empty);
            Assert.That(noteEvt.History, Is.Empty);
        }
    }
}
