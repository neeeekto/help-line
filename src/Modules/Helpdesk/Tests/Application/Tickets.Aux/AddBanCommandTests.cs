using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.AddBan;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetBans;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class AddBanCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(AddBanCommandTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
            await CreateOperator();
        }

        [Test]
        public async Task AddBanCommand_WhenIsValid_IsSuccessful()
        {
            var banValue = "127.0.0.1";
            var banId = await Module.ExecuteCommandAsync(new AddBanCommand(ProjectId, Ban.Parameters.Ip, banValue));

            var entities = await Module.ExecuteQueryAsync(new GetBansQuery(ProjectId));
            Assert.AreEqual(1, entities.Count());
            var entity = entities.FirstOrDefault(x => x.Id == banId);
            Assert.That(entity.Value, Is.EqualTo(banValue));
            Assert.That(entity.ExpiredAt, Is.Null);
            Assert.That(entity.ProjectId, Is.EqualTo(ProjectId));
            Assert.That(entity.Parameter, Is.EqualTo(Ban.Parameters.Ip));
        }

        public class InvalidSource
        {
            public string ProjectId = TicketAuxTestBase.ProjectId;
            public Ban.Parameters Parameter = Ban.Parameters.Text;
            public string Value = "127.0.0.1";
            public DateTime? ExpiredAt = null;
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
                            ExpiredAt = DateTime.UtcNow - new TimeSpan(2)
                        }
                    ).SetName("Invalid ExpiredAt");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Value = null
                        }
                    ).SetName("Value: null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Value = ""
                        }
                    ).SetName("Value: empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Parameter = Ban.Parameters.Ip,
                            Value = "test"
                        }
                    ).SetName("Value: invalid ip");

                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task AddBanCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new AddBanCommand(src.ProjectId, src.Parameter, src.Value, src.ExpiredAt);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
