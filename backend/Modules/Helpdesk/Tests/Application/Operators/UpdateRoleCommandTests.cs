using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.CreateRole;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.UpdateRole;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;
using HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetRoles;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Operators
{
    [TestFixture]
    [NonParallelizable]
    public class UpdateRoleCommandTests : OperatorsTestBase
    {
        protected override string NS => nameof(UpdateRoleCommandTests);


        [Test]
        public async Task UpdateOperatorRoleWithCorrectData()
        {
            var data = new OperatorRoleData()
            {
                Title = "Test"
            };
            var roleId = await Module.ExecuteCommandAsync(new CreateRoleCommand(data));

            data.Title = "Test2";
            await Module.ExecuteCommandAsync(new UpdateRoleCommand(roleId, data));

            var role = await Module.ExecuteQueryAsync(new GetRoleQuery(roleId));
            Assert.That(role.Data.Title, Is.EqualTo(data.Title));
        }

        [Test]
        public async Task UpdateOperatorRoleNotAffectOtherRoles()
        {
            var data1 = new OperatorRoleData()
            {
                Title = "Test1"
            };

            var data2 = new OperatorRoleData()
            {
                Title = "Test2"
            };
            var roleId1 = await Module.ExecuteCommandAsync(new CreateRoleCommand(data1));
            var roleId2 = await Module.ExecuteCommandAsync(new CreateRoleCommand(data1));

            await Module.ExecuteCommandAsync(new UpdateRoleCommand(roleId2, data2));

            var role1 = await Module.ExecuteQueryAsync(new GetRoleQuery(roleId1));
            var role2 = await Module.ExecuteQueryAsync(new GetRoleQuery(roleId2));
            Assert.That(role1.Data.Title, Is.EqualTo(data1.Title));
            Assert.That(role2.Data.Title, Is.EqualTo(data2.Title));
        }

        [Test]
        public async Task UpdateNotExistsOperatorRole_ThrowNotFoundException()
        {
            var data = new OperatorRoleData()
            {
                Title = "Test"
            };
            Assert.CatchAsync<NotFoundException>(() =>
                Module.ExecuteCommandAsync(new UpdateRoleCommand(Guid.NewGuid(), data)));
        }

        public class InvalidSource
        {
            public string Title = "test";

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
            var cmd = new UpdateRoleCommand(Guid.NewGuid(), new OperatorRoleData()
            {
                Title = src.Title
            });
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
