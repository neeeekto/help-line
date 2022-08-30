using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.RemovePlatform;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SavePlatform;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Queries.GetPlatforms;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.CreationOptions
{
    [NonParallelizable]
    [TestFixture]
    public class RemovePlatformCommandTests : CreationOptionsTestBase
    {
        protected override string NS => nameof(RemovePlatformCommandTests);
        public const string Key = "test";
        public const string Name = "test";
        public const string Icon = "test";

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
            await Module.ExecuteCommandAsync(new SavePlatformCommand(Key, ProjectId, Name, Icon));
        }

        [Test]
        public async Task RemovePlatformCommand_WhenDataIsValid_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new RemovePlatformCommand(Key, ProjectId));
            var entities = await Module.ExecuteQueryAsync(new GetPlatformsQuery(ProjectId));
            var entity = entities.FirstOrDefault(x => x.Key == Key);
            Assert.IsNull(entity);
        }

        [Test]
        public async Task RemovePlatformCommand_WhenExistPlatformInDifferentProjects_IsSuccessful()
        {
            var otherProjectId = "project2";
            await CreateProject(projectId: otherProjectId);
            await Module.ExecuteCommandAsync(new SavePlatformCommand(Key, otherProjectId, Name, Icon));

            await Module.ExecuteCommandAsync(new RemovePlatformCommand(Key, ProjectId));
            var entities = await Module.ExecuteQueryAsync(new GetPlatformsQuery(otherProjectId));
            var entity = entities.FirstOrDefault(x => x.Key == Key);
            Assert.IsNotNull(entity);
        }

        [Test]
        public async Task RemovePlatformCommand_WhenProjectNotExist_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new RemovePlatformCommand(Key, "not exist"));
            var entities = await Module.ExecuteQueryAsync(new GetPlatformsQuery(ProjectId));
            var entity = entities.FirstOrDefault(x => x.Key == Key);
            Assert.IsNotNull(entity);
        }

        [Test]
        public async Task RemovePlatformCommand_WhenPlatformNotExist_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new RemovePlatformCommand(ProjectId, "not exist"));
        }
    }
}
