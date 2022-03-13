using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork;
using HelpLine.Modules.Helpdesk.Jobs;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets
{
    public class ReopenTicketCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(ReopenTicketCommandTests);

        [Test]
        public async Task ReopenTicketCommand_WhenDataIsValid_IsSuccessful()
        {
            await BusServiceFactory.PublishInQueues(new RunTicketTimersJob(Guid.NewGuid()));
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            await SetDelayConfigForTests();
            await Reply(ticketId, new MessageDto() {Text = "test"}, new SystemInitiatorDto());
            await BusServiceFactory.PublishInQueues(new RunTicketTimersJob(Guid.NewGuid()));
            await BusServiceFactory.EmitAllQueues(); // because timer is internal command
            await ExecuteAction(ticketId, new ReopenTicketAction(), new SystemInitiatorDto());

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketStatusChangedEventView>().LastOrDefault();

            Assert.IsNotNull(evt);
            Assert.That(ticketView.Status.Kind, Is.EqualTo(TicketStatusKind.Opened));
            Assert.That(ticketView.Status.Type, Is.EqualTo(TicketStatusType.Answered));

            Assert.That(evt.New.Kind, Is.EqualTo(TicketStatusKind.Opened));
            Assert.That(evt.New.Type, Is.EqualTo(TicketStatusType.Answered));
        }
    }
}
