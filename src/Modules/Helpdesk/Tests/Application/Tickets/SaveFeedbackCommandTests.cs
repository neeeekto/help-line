using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.SaveFeedback;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Jobs;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    public class SaveFeedbackCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(SaveFeedbackCommandTests);

        [Test]
        public async Task SaveFeedbackCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            await SetDelayConfigForTests();
            await Reply(ticketId, new MessageDto() {Text = "test"}, new SystemInitiatorDto());
            await BusServiceFactory.PublishInQueues(new RunTicketTimersJob(Guid.NewGuid()));
            await BusServiceFactory.EmitAllQueues();
            var feedbackId =
                await ExecuteAction(ticketId, new ImmediateSendFeedbackAction(), new SystemInitiatorDto());

            await Module.ExecuteCommandAsync(new SaveFeedbackCommand(ticketId, ((TicketFeedbackId) feedbackId).Value!,
                new TicketFeedbackDto {Score = 5}, testData.UserId));

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            Assert.That(ticketView.Feedbacks.Count(), Is.EqualTo(1));
        }
    }
}
