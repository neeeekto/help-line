using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetBanSettings;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class CreateBanSettingsByProjectCreatedNotificationHandlerTests : TicketAuxTestBase
    {
        protected override string NS => nameof(CreateBanSettingsByProjectCreatedNotificationHandlerTests);

        [Test]
        public async Task CreateDefaultTicketConfigurationCommand_IsSuccessful()
        {
            await CreateProject();
            var banSettings = await Module.ExecuteQueryAsync(new GetBanSettingsQuery(ProjectId));
            Assert.That(banSettings, Is.Not.Null);
            Assert.That(banSettings.ProjectId, Is.EqualTo(ProjectId));
        }
    }
}
