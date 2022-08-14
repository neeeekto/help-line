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
    public class ChangeTagsCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(ChangeTagsCommandTests);

        [Test]
        public async Task ChangeTagsCommand_WhenDataIsValid_IsSuccessful()
        {
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);

            var tag = "new-tag";

            var cmd = new ChangeTagsAction(tag);
            await ExecuteAction(ticketId, cmd, new SystemInitiatorDto());

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketTagsChangedEventView>().FirstOrDefault();

            Assert.IsNotNull(evt);
            Assert.IsNotNull(evt.CreateDate);
            Assert.IsTrue(ticketView.Tags.Contains(tag));
            Assert.IsFalse(ticketView.Tags.Contains(testData.Tag));
            Assert.AreEqual(1, ticketView.Tags.Count());
            Assert.AreEqual(1, evt.Old.Count());
            Assert.IsTrue(evt.Old.Contains(testData.Tag));
            Assert.IsFalse(evt.New.Contains(testData.Tag));
            Assert.IsTrue(evt.New.Contains(tag));
        }
    }
}
