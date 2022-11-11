using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    public class RejectTicketCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(RejectTicketCommandTests);

        [Test]
        public async Task RejectTicketCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);

            await ExecuteAction(ticketId, new RejectTicketAction(), new SystemInitiatorDto());

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));

            Assert.AreEqual(TicketStatusKind.Closed, ticketView.Status.Kind);
            Assert.AreEqual(TicketStatusType.Rejected, ticketView.Status.Type);
        }
    }
}
