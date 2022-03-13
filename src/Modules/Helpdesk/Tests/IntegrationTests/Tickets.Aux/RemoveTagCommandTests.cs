using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTag;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTag;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTags;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class RemoveTagCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(RemoveTagCommandTests);

        [Test]
        public async Task RemoveTagCommand_WhenDataIsValid_IsSuccessful()
        {
            await CreateProject();
            var tagKey = "test";
            await Module.ExecuteCommandAsync(new SaveTagCommand(tagKey, ProjectId, true));
            await Module.ExecuteCommandAsync(new RemoveTagCommand(tagKey, ProjectId));
            var tags = await Module.ExecuteQueryAsync(new GetTagsQuery(ProjectId));
            var tag = tags.FirstOrDefault(x => x.Key == tagKey);
            Assert.IsNull(tag);
        }

        [Test]
        public async Task RemoveTagCommand_WhenExistTagInDifferentProjects_IsSuccessful()
        {
            var otherProjectId = "project2";
            await CreateProject();
            await CreateProject(projectId: otherProjectId);
            var tagKey = "test";
            await Module.ExecuteCommandAsync(new SaveTagCommand(tagKey, ProjectId, true));
            await Module.ExecuteCommandAsync(new SaveTagCommand(tagKey, otherProjectId, true));
            var cmd = new RemoveTagCommand(tagKey, ProjectId);
            await Module.ExecuteCommandAsync(cmd);
            var tags = await Module.ExecuteQueryAsync(new GetTagsQuery(otherProjectId));
            var tag = tags.FirstOrDefault(x => x.Key == tagKey);
            Assert.IsNotNull(tag);
        }

        [Test]
        public async Task RemoveTagCommand_WhenProjectNotExist_IsSuccessful()
        {
            await CreateProject();
            var tagKey = "test";
            await Module.ExecuteCommandAsync(new SaveTagCommand(tagKey, ProjectId, true));
            await Module.ExecuteCommandAsync(new RemoveTagCommand(tagKey, "not exist"));
            var tags = await Module.ExecuteQueryAsync(new GetTagsQuery(ProjectId));
            var tag = tags.FirstOrDefault(x => x.Key == tagKey);
            Assert.IsNotNull(tag);
        }

        [Test]
        public async Task RemoveTagCommand_WhenTagNotExist_IsSuccessful()
        {
            await CreateProject();
            await Module.ExecuteCommandAsync(new RemoveTagCommand(ProjectId, "not exist"));
        }
    }
}
