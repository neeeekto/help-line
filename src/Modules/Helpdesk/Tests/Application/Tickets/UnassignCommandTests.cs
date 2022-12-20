using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    [TestFixture]
    public class UnassignCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(UnassignCommandTests);

        [Test]
        public async Task UnassignCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            var operatorId = Guid.NewGuid();
            await CreateOperator(operatorId);
            await ExecuteAction(ticketId, new AssignAction(operatorId), new SystemInitiatorDto());
            await ExecuteAction(ticketId, new UnassignAction(), new SystemInitiatorDto());

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketAssigmentEventView>().LastOrDefault();

            Assert.NotNull(evt);
            Assert.IsNull(ticketView.AssignedTo);
            Assert.IsNull(evt.To);
            Assert.AreEqual(operatorId, evt.From);
        }
    }
}
