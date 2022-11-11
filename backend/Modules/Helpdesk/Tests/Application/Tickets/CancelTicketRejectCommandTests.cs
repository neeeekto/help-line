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
    public class CancelTicketRejectCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(CancelTicketRejectCommandTests);

        [Test]
        public async Task CancelTicketRejectCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            await Reply(ticketId, new MessageDto() {Text = "test"},
                new SystemInitiatorDto()); // new tickets rejecting without approve

            var msg = "reject!";
            await Module.ExecuteCommandAsync(new ExecuteTicketActionCommand(ticketId, new RejectTicketAction(
                msg), new SystemInitiatorDto())); // message required - see domain logic

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketApprovalStatusEventView>().FirstOrDefault();
            var cmd = new CancelTicketRejectAction(evt.RejectId);
            await Module.ExecuteCommandAsync(new ExecuteTicketActionCommand(ticketId, cmd, new SystemInitiatorDto()));

            ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            evt = ticketView.Events.OfType<TicketApprovalStatusEventView>().FirstOrDefault();

            Assert.IsNotNull(evt);
            Assert.IsNotNull(evt.CreateDate);
            Assert.AreEqual(msg, evt.Message);
            Assert.AreEqual(TicketApprovalStatusEventView.ApproveState.Canceled, evt.State);
            Assert.AreEqual(TicketStatusKind.Opened, evt.ForStatus.Kind);
        }
    }
}
