using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.CreateTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Aux
{
    public class CreateTicketIdCounterCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(CreateTicketIdCounterCommandTests);

        [Test]
        public async Task CounterCreated_AfterProjectCreation_Success()
        {
            await CreateProject();

            var ticketId = await Module.ExecuteCommandAsync(new CreateTicketCommand(ProjectId, "en", new SystemInitiatorDto(),
                new string[] { }, new Dictionary<string, string>() {{"1", "slack"}},
                new Dictionary<string, string>() { }, null, null, "test", null));
            Assert.IsTrue(ticketId.Contains("1-0000001"));
        }
    }
}
