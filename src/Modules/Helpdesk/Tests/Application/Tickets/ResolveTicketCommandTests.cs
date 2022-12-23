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
    [TestFixture]
    public class ResolveTicketCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(ResolveTicketCommandTests);

        [Test]
        public async Task ResolveTicketCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            await Reply(ticketId, new MessageDto() {Text = "test"}, new SystemInitiatorDto());
            await ExecuteAction(ticketId, new ResolveTicketAction(), new SystemInitiatorDto());
            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketStatusChangedEventView>().LastOrDefault();

            Assert.IsNotNull(evt);
            Assert.That(ticketView.Status.Kind, Is.EqualTo(TicketStatusKind.Opened));
            Assert.That(ticketView.Status.Type, Is.EqualTo(TicketStatusType.Resolved));

            Assert.That(evt.New.Kind, Is.EqualTo(TicketStatusKind.Opened));
            Assert.That(evt.New.Type, Is.EqualTo(TicketStatusType.Resolved));
        }
    }
}
