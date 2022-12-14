using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.CreateRole;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.DeleteRole;
using HelpLine.Modules.Helpdesk.Application.Operators.Commands.SetOperatorRoles;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;
using HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetOperator;
using HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetRoles;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Operators
{
    [TestFixture]
    public class DeleteRoleCommandTests : OperatorsTestBase
    {
        protected override string NS => nameof(DeleteRoleCommandTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateOperator();
            await CreateProject();
        }

        [Test]
        public async Task DeleteExistsRole()
        {
            var data = new OperatorRoleData()
            {
                Title = "Test"
            };
            var roleId = await Module.ExecuteCommandAsync(new CreateRoleCommand(data));
            await Module.ExecuteCommandAsync(new DeleteRoleCommand(roleId));

            var role = await Module.ExecuteQueryAsync(new GetRoleQuery(roleId));
            Assert.That(role, Is.Null);
        }

        [Test]
        public async Task DeleteNotExistsRole()
        {
            var data = new OperatorRoleData()
            {
                Title = "Test"
            };
            var roleId = await Module.ExecuteCommandAsync(new CreateRoleCommand(data));
            await Module.ExecuteCommandAsync(new DeleteRoleCommand(Guid.NewGuid()));

            var role = await Module.ExecuteQueryAsync(new GetRoleQuery(roleId));
            Assert.That(role, Is.Not.Null);
        }

        [Test]
        public async Task DeleteUsedRole()
        {
            var data = new OperatorRoleData()
            {
                Title = "Test"
            };
            var roleId = await Module.ExecuteCommandAsync(new CreateRoleCommand(data));
            await Module.ExecuteCommandAsync(new SetOperatorRolesCommand(OperatorId, ProjectId, new [] {roleId}));
            Assert.CatchAsync<InvalidCommandException>(async () => await Module.ExecuteCommandAsync(new DeleteRoleCommand(roleId)));
        }

    }
}
