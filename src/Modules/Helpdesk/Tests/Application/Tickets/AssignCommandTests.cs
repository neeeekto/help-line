using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Actions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.ExecuteTicketAction;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    [TestFixture]
    public class AssignCommandTests : TicketsTestBase
    {
        protected override string NS => nameof(AssignCommandTests);

        [Test]
        public async Task AssignCommandCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();
            var testData = new TicketTestData();
            var operatorId = Guid.NewGuid();
            await CreateOperator(operatorId);
            var ticketId = await CreateTicket(testData);

            var act = new AssignAction(operatorId);
            await Module.ExecuteCommandAsync(new ExecuteTicketActionCommand(ticketId, act, new SystemInitiatorDto()));

            var ticketView = await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId));
            var evt = ticketView.Events.OfType<TicketAssigmentEventView>().FirstOrDefault();

            Assert.IsNotNull(evt);
            Assert.IsNotNull(evt.CreateDate);
            Assert.AreEqual(operatorId, ticketView.AssignedTo);
            Assert.IsNull(evt.From);
            Assert.AreEqual(operatorId, evt.To);
        }
    }
}
