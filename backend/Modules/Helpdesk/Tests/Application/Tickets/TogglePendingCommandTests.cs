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
    [TestFixture]
    public class TogglePendingCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(TogglePendingCommandTests);

        [Test]
        public async Task TogglePendingCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            var operatorId = Guid.NewGuid();
            await CreateOperator(operatorId);
            await ExecuteAction(ticketId, new AssignAction(operatorId), new SystemInitiatorDto());
            await ExecuteAction(ticketId, new TogglePendingAction(true), new SystemInitiatorDto());

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketStatusChangedEventView>().FirstOrDefault();

            Assert.NotNull(evt);
            Assert.That(evt.New.Kind, Is.EqualTo(TicketStatusKind.Pending));
            Assert.That(ticketView.Status.Kind, Is.EqualTo(TicketStatusKind.Pending));
        }
    }
}
