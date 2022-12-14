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
    public class AddOutgoingMessageCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(AddOutgoingMessageCommandTests);

        [Test]
        public async Task AddOutgoingMessageCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            var message = new MessageDto {Text = Guid.NewGuid().ToString()};
            var action = new AddOutgoingMessageAction(message);
            var messageId = await Module.ExecuteCommandAsync(new ExecuteTicketActionCommand(
                ticketId,
                action,
                new SystemInitiatorDto()
            ));

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var messageEvt = ticketView.Events.OfType<TicketOutgoingMessageEventView>()
                .FirstOrDefault(x => x.Message.Text == action.Message.Text);

            Assert.AreEqual(1, ticketView.DiscussionState.IterationCount);
            Assert.AreEqual(TicketDiscussionStateView.MessageType.Outgoin, ticketView.DiscussionState.LastMessageType);

            Assert.IsNotNull(messageEvt);
            Assert.AreEqual(messageEvt.Message.Text, message.Text);
            Assert.IsInstanceOf<SystemInitiatorView>(messageEvt.Initiator);
            Assert.IsTrue(messageEvt.Recipients.Any(x => x.UserId == testData.UserId && x.Channel == testData.Channel));
            Assert.IsTrue(messageEvt.Recipients.Any(x => x.DeliveryStatuses.Any(x =>
                x.Status == MessageStatus.Sending && x.Date <= DateTime.UtcNow && x.Detail == null)));
            Assert.IsTrue(messageEvt.Recipients.Any(x => x.DeliveryStatuses.Count() == 1));
        }
    }
}
