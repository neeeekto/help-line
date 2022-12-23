using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.ExecuteTicketAction;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    [TestFixture]
    public class AddUserIdCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(AddUserIdCommandTests);

        [Test]
        public async Task AddUserIdCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();
            var testData = new TicketTestData();
            var ticketId = await CreateTicket(testData);
            var userId = "userid";
            var channel = Channels.Email;
            var cmd = new AddUserIdAction( userId, channel, false, false);
            await Module.ExecuteCommandAsync(new ExecuteTicketActionCommand(ticketId, cmd, new SystemInitiatorDto()));

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketUserIdsChangedEventView>().FirstOrDefault();

            Assert.IsNotNull(evt);
            Assert.IsNotNull(evt.CreateDate);
            Assert.IsInstanceOf<SystemInitiatorView>(evt.Initiator);
            Assert.IsTrue(evt.Old.Count() == 1);
            Assert.IsTrue(evt.New.Count() == 2);
            Assert.IsTrue(evt.New.Any(x =>
                x.UseForDiscussion == cmd.UseForDiscussion && x.Channel == cmd.Channel && x.Type == UserIdType.Linked &&
                x.UserId == cmd.UserId));

            Assert.IsTrue(ticketView.UserIds.Count() == 2);
            Assert.IsTrue(ticketView.UserIds.Any(x =>
                x.UseForDiscussion == cmd.UseForDiscussion && x.Channel == cmd.Channel && x.Type == UserIdType.Linked &&
                x.UserId == cmd.UserId));
        }
    }
}
