using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetBanSetting;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetBans;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Aux
{
    [TestFixture]
    public class CheckUserNeedBannedByIpCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(CheckUserNeedBannedByIpCommandTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
            await CreateOperator();
        }

        [Test]
        public async Task CheckUserNeedBannedByIpCommand_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new SetBanSettingCommand(
                ProjectId,
                new BanSettings()
                {
                    Interval = new TimeSpan(TimeSpan.TicksPerHour),
                    BanDelay = new TimeSpan(TimeSpan.TicksPerHour),
                    TicketsCount = 1
                }));

            var testIp = "127.0.0.1";
            var ticketData = new TicketTestData();
            ticketData.UserMeta["ip"] = testIp;
            await CreateTicket(ticketData);
            await BusServiceFactory.EmitAll();
            await BusServiceFactory.EmitAll();
            var entities = await Module.ExecuteQueryAsync(new GetBansQuery(ProjectId));
            Assert.That(entities, Has.One.Items);

            var ban = entities.FirstOrDefault();
            Assert.That(ban.Value, Is.EqualTo(testIp));
            Assert.That(ban.Parameter, Is.EqualTo(Ban.Parameters.Ip));
            Assert.That(ban.ExpiredAt,
                Is.GreaterThan(DateTime.UtcNow + new TimeSpan(TimeSpan.TicksPerHour - TimeSpan.TicksPerMinute)));
        }
    }
}
