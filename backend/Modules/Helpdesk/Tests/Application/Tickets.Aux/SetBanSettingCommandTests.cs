using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetBanSetting;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetBanSettings;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Aux
{
    [TestFixture]
    public class SetBanSettingCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(SetBanSettingCommandTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        private BanSettings MakeData() => new BanSettings
        {
            Interval = new TimeSpan(TimeSpan.TicksPerDay),
            BanDelay = new TimeSpan(TimeSpan.TicksPerDay),
            ProjectId = ProjectId,
            TicketsCount = 1
        };

        [Test]
        public async Task SetBanSettingCommand_WhenIsValid_IsSuccessful()
        {
            var data = MakeData();
            await Module.ExecuteCommandAsync(new SetBanSettingCommand(ProjectId, data));

            var entity = await Module.ExecuteQueryAsync(new GetBanSettingsQuery(ProjectId));

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.ProjectId, Is.EqualTo(ProjectId));
            Assert.That(entity.Interval, Is.EqualTo(data.Interval));
            Assert.That(entity.BanDelay, Is.EqualTo(data.BanDelay));
            Assert.That(entity.TicketsCount, Is.EqualTo(data.TicketsCount));
        }

        public class InvalidSource : BanSettings
        {
            public string ProjectId = HelpdeskTestBase.ProjectId;

            public InvalidSource()
            {
                Interval = new TimeSpan(1);
                BanDelay = new TimeSpan(1);
                TicketsCount = 1;
            }

            public static IEnumerable<TestCaseData> Cases
            {
                get
                {
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = ""
                        }
                    ).SetName("Empty project: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = null
                        }
                    ).SetName("Empty project: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = "asd"
                        }
                    ).SetName("Not exist project");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Interval = new TimeSpan(0)
                        }
                    ).SetName("Invalid Interval");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            BanDelay = new TimeSpan(0)
                        }
                    ).SetName("Invalid BanDelay");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            TicketsCount = 0
                        }
                    ).SetName("Invalid TicketsCount");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task SetBanSettingCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new SetBanSettingCommand(src.ProjectId, src);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
