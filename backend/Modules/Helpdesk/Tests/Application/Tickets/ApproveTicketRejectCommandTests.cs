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
    public class ApproveTicketRejectCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(ApproveTicketRejectCommandTests);

        [Test]
        public async Task ApproveTicketRejectCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            await Reply(ticketId, new MessageDto() {Text = "test"},
                new SystemInitiatorDto()); // new tickets rejecting without approve

            var msg = "reject!";
            var action1 = new RejectTicketAction(msg);
            await Module.ExecuteCommandAsync(new ExecuteTicketActionCommand(ticketId, action1, new SystemInitiatorDto()));

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketApprovalStatusEventView>().FirstOrDefault();
            var action2 = new ApproveTicketRejectAction(evt.RejectId);
            await Module.ExecuteCommandAsync(new ExecuteTicketActionCommand(ticketId, action2, new SystemInitiatorDto()));

            ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            evt = ticketView.Events.OfType<TicketApprovalStatusEventView>().FirstOrDefault();

            Assert.IsNotNull(evt);
            Assert.IsNotNull(evt.CreateDate);
            Assert.AreEqual(msg, evt.Message);
            Assert.AreEqual(TicketApprovalStatusEventView.ApproveState.Approved, evt.State);
            Assert.AreEqual(TicketStatusKind.Opened, evt.ForStatus.Kind);
        }
    }
}
