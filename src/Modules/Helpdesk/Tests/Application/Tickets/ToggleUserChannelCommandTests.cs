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
    public class ToggleUserChannelCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(ToggleUserChannelCommandTests);

        [Test]
        public async Task ToggleUserChannelCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            var userId = "test2";
            await ExecuteAction(ticketId, new AddUserIdAction(userId, testData.Channel, true, true),
                new SystemInitiatorDto());
            await ExecuteAction(ticketId, new ToggleUserChannelAction(testData.UserId, false),
                new SystemInitiatorDto());

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketUserIdsChangedEventView>().LastOrDefault();

            Assert.NotNull(evt);
            Assert.IsTrue(evt.New.Any(x => x.UserId == testData.UserId && x.UseForDiscussion == false));
        }
    }
}
