using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.CreateRole;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;
using HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetRoles;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Operators
{
    [TestFixture]
    [NonParallelizable]
    public class CreateRoleCommandTests : OperatorsTestBase
    {
        protected override string NS => nameof(CreateRoleCommandTests);


        [Test]
        public async Task CreateOperatorRoleWithCorrectData()
        {
            var data = new OperatorRoleData()
            {
                Title = "Test"
            };
            var roleId = await Module.ExecuteCommandAsync(new CreateRoleCommand(data));

            var role = await Module.ExecuteQueryAsync(new GetRoleQuery(roleId));
            Assert.That(role.Id, Is.EqualTo(roleId));
            Assert.That(role.Data.Title, Is.EqualTo(data.Title));
        }

        public class InvalidSource
        {
            public string Title = "";

            public static IEnumerable<TestCaseData> Cases
            {
                get
                {
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Title = null
                        }
                    ).SetName("Empty title: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Title = ""
                        }
                    ).SetName("Empty title: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Title = " "
                        }
                    ).SetName("Empty title: Spaces");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task CreateOperatorRoleCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new CreateRoleCommand(new OperatorRoleData()
            {
                Title = src.Title
            });
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
