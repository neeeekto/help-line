using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.AddMessageStatus;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.RetryOutgoingMessage;
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
    public class RetryOutgoingMessageCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(RetryOutgoingMessageCommandTests);

        [Test]
        public async Task RetryOutgoingMessageCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            var messageId = await Reply(ticketId, new MessageDto() {Text = "test"}, new SystemInitiatorDto());
            await Module.ExecuteCommandAsync(new AddMessageStatusCommand(ticketId, messageId,
                MessageStatus.NotDelivered,
                testData.UserId));
            await Module.ExecuteCommandAsync(new RetryOutgoingMessageCommand(ticketId, messageId,
                testData.UserId, new SystemInitiatorDto()));
            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketOutgoingMessageEventView>().LastOrDefault();

            Assert.IsNotNull(evt);

            var recipient = evt.Recipients.First(x => x.UserId == testData.UserId);
            Assert.That(recipient.DeliveryStatuses.Count(), Is.EqualTo(3));
        }
    }
}
