using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.AddBan;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets
{
    [TestFixture]
    public class TicketCheckerTests : TicketsTestBase
    {
        protected override string NS => nameof(TicketCheckerTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        [Test]
        public async Task BanCheck_ByIp_IsSuccessful()
        {
            var testIp = "127.0.0.1";
            await Module.ExecuteCommandAsync(new AddBanCommand(ProjectId, Ban.Parameters.Ip, testIp));
            var testData = new TicketTestData()
            {
                UserMeta = new Dictionary<string, string>()
                {
                    {"IP", testIp}
                }
            };
            Assert.CatchAsync(() => CreateTicket(testData));
        }

        [Test]
        public async Task BanCheck_ByMessage_IsSuccessful()
        {
            var testMessage = "test";
            await Module.ExecuteCommandAsync(new AddBanCommand(ProjectId, Ban.Parameters.Text, testMessage));
            var testData = new TicketTestData();
            testData.Message = new MessageDto()
            {
                Text = "test test"
            };
            Assert.CatchAsync(() => CreateTicket(testData));
        }
    }
}
