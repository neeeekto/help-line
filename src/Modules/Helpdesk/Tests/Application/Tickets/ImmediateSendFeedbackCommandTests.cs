using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Jobs;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    public class ImmediateSendFeedbackCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(ImmediateSendFeedbackCommandTests);

        [Test]
        public async Task ImmediateSendFeedbackCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            await SetDelayConfigForTests();
            await Reply(ticketId, new MessageDto() {Text = "test"}, new SystemInitiatorDto());
            await BusServiceFactory.PublishInQueues(new RunTicketTimersJob(Guid.NewGuid()));
            await BusServiceFactory.EmitAllQueues();

            var feedbackId = await ExecuteAction(ticketId, new ImmediateSendFeedbackAction(), new SystemInitiatorDto());

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketFeedbackEventView>().SingleOrDefault();

            Assert.IsNotNull(evt);
            Assert.IsNotNull(evt.CreateDate);
            Assert.IsNotNull(evt.Id);
            Assert.AreEqual(((TicketFeedbackId) feedbackId).Value, evt.FeedbackId);
            Assert.IsInstanceOf<SystemInitiatorView>(evt.Initiator);
        }
    }
}
