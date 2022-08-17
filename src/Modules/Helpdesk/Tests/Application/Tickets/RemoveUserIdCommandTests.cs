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
    public class RemoveUserIdCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(RemoveUserIdCommandTests);

        [Test]
        public async Task RemoveUserIdCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);

            var userId = "test2";
            await ExecuteAction(ticketId, new AddUserIdAction(userId, "email", true, true), new SystemInitiatorDto());
            await ExecuteAction(ticketId, new RemoveUserIdAction(testData.UserId), new SystemInitiatorDto());

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketUserIdsChangedEventView>().LastOrDefault();

            Assert.IsNotNull(evt);
            Assert.IsTrue(evt.Old.Any(x => x.UserId == testData.UserId));
            Assert.IsFalse(evt.New.Any(x => x.UserId == testData.UserId));

            Assert.IsTrue(ticketView.UserIds.Any(x => x.UserId == userId));
            Assert.IsFalse(ticketView.UserIds.Any(x => x.UserId == testData.UserId));
        }
    }
}
