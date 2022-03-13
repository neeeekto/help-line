using System;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.AddBan;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveBan;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetBans;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class RemoveBanCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(RemoveBanCommandTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        private Task<Guid> CreateBan(string? projectId = null)
        {
            return Module.ExecuteCommandAsync(new AddBanCommand(projectId ?? ProjectId, Ban.Parameters.Text, TestStr));
        }


        [Test]
        public async Task RemoveBanCommand_WhenIsValid_IsSuccessful()
        {
            var banId = await CreateBan();
           await Module.ExecuteCommandAsync(new RemoveBanCommand(banId));

            var entities = await Module.ExecuteQueryAsync(new GetBansQuery(ProjectId));
            Assert.That(entities, Is.Empty);
        }

        [Test]
        public async Task RemoveBanCommand_WhenNotExist_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new RemoveBanCommand(Guid.NewGuid()));
            var entities = await Module.ExecuteQueryAsync(new GetBansQuery(ProjectId));
            Assert.That(entities, Is.Empty);
        }
    }
}
