using System.Collections.Generic;
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
    public class ChangeUserMetaCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(ChangeUserMetaCommandTests);

        [Test]
        public async Task ChangeUserMetaCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);

            var meta = new Dictionary<string, string>()
            {
                {"test1", "test2"}
            };
            var cmd = new ChangeUserMetaAction( meta);
            await ExecuteAction(ticketId, cmd, new SystemInitiatorDto());

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketUserMetaChangedEventView>().SingleOrDefault();

            Assert.IsTrue(ticketView.UserMeta.ContainsKey("test1"));
            Assert.IsTrue(ticketView.UserMeta.ContainsValue("test2"));

            Assert.IsNotNull(evt);
            Assert.IsNotNull(evt.CreateDate);
            Assert.IsTrue(evt.Old.ContainsKey("test"));
            Assert.IsTrue(evt.New.ContainsKey("test1"));
        }
    }
}
