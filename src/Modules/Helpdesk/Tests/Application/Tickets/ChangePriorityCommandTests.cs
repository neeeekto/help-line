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
    public class ChangePriorityCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(ChangePriorityCommandTests);

        [Test]
        public async Task ChangePriorityCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);

            var cmd = new ChangePriorityAction(TicketPriority.High);
            await ExecuteAction(ticketId, cmd, new SystemInitiatorDto());

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketPriorityEventView>().FirstOrDefault();

            Assert.IsNotNull(evt);
            Assert.IsNotNull(evt.CreateDate);
            Assert.AreEqual(TicketPriority.High, ticketView.Priority);
            Assert.AreEqual(TicketPriority.Normal, evt.Old);
            Assert.AreEqual(TicketPriority.High, evt.New);
        }
    }
}
