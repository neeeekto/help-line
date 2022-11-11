using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.CreateRole;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.SetOperatorRoles;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;
using HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetOperator;
using HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetRoles;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using Nest;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Operators
{
    [TestFixture]
    [NonParallelizable]
    public class SetOperatorRolesCommandTests : OperatorsTestBase
    {
        protected override string NS => nameof(SetOperatorRolesCommandTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateOperator();
            await CreateProject();
            await CreateRole();
        }

        [Test]
        public async Task SetOperatorRolesCommand_WhenDataIsValid_IsSuccessful()
        {
            var roles = await Module.ExecuteQueryAsync(new GetRolesQuery());
            await Module.ExecuteCommandAsync(new SetOperatorRolesCommand(OperatorId, ProjectId, roles.Select(x => x.Id)));
            var oper = await Module.ExecuteQueryAsync(new GetOperatorQuery(OperatorId));
            Assert.True(roles.All(x => oper.Roles[ProjectId].Contains(x.Id)));
        }

        [Test]
        public async Task SetOperatorRolesCommand_WhenRoleDoesntExists_ThrowInvalidCommand()
        {
            Assert.CatchAsync<InvalidCommandException>(() =>
                Module.ExecuteCommandAsync(new SetOperatorRolesCommand(OperatorId, ProjectId, new[] {Guid.NewGuid(),})));
        }



        private async Task CreateRole()
        {
            await Module.ExecuteCommandAsync(new CreateRoleCommand(new OperatorRoleData() {Title = "test"}));
        }

        public class InvalidSource
        {
            public Guid OperatorId = Guid.NewGuid();
            public string ProjectId = HelpdeskTestBase.ProjectId;
            public IEnumerable<Guid> RoleIds = new []{Guid.NewGuid()};

            public static IEnumerable<TestCaseData> Cases
            {
                get
                {
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            OperatorId = Guid.Empty
                        }
                    ).SetName("Empty operator id: Guid.Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            RoleIds = null
                        }
                    ).SetName("Empty role ids: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            RoleIds = Array.Empty<Guid>()
                        }
                    ).SetName("Empty role ids: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            RoleIds = new []{Guid.Empty}
                        }
                    ).SetName("Empty role ids: Empty role id");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = ""
                        }
                    ).SetName("Empty project: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = "111"
                        }
                    ).SetName("Project: Not exist");

                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task SetOperatorRolesCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new SetOperatorRolesCommand(src.OperatorId, ProjectId, src.RoleIds);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
