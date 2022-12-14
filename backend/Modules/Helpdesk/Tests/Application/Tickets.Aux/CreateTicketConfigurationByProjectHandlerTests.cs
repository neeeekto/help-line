using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketsDelayConfiguration;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Aux
{
    [TestFixture]
    public class CreateTicketConfigurationByProjectHandlerTests : TicketAuxTestBase
    {
        protected override string NS => nameof(CreateTicketConfigurationByProjectHandlerTests);

        [Test]
        public async Task CreateDefaultTicketConfigurationCommand_IsSuccessful()
        {
            await CreateProject();
            var configuration = await Module.ExecuteQueryAsync(new GetTicketsDelayConfigurationQuery(ProjectId));
            Assert.That(configuration, Is.Not.Null);
            Assert.That(configuration.ProjectId, Is.EqualTo(ProjectId));
        }
    }
}
