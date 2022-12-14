using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.RemoveProblemAndTheme;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SavePlatform;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SaveProblemAndTheme;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Queries.GetProblemAndTheme;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTag;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.CreationOptions
{
    [TestFixture]
    public class RemoveProblemAndThemeCommandTests : CreationOptionsTestBase
    {
        protected override string NS => nameof(RemoveProblemAndThemeCommandTests);
        public const string ExistTag = "test";

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
            await Module.ExecuteCommandAsync(new SaveTagCommand(ExistTag, ProjectId, true));
            await Module.ExecuteCommandAsync(new SavePlatformCommand("test", ProjectId, "test", "test"));
            await Module.ExecuteCommandAsync(new SaveProblemAndThemeCommand(ProjectId, ExistTag, new ProblemAndTheme
            {
                Enabled = true,
                Subtypes = null,
                Platforms = new[] {ExistTag},
                Content = new LocalizeDictionary<ProblemAndThemeContent>()
                {
                    {"en", new ProblemAndThemeContent {Notification = "test", Title = "test"}}
                }
            }));
        }

        [Test]
        public async Task RemoveProblemAndThemeCommandTests_WhenDataIsValid_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new RemoveProblemAndThemeCommand(ExistTag, ProjectId));
            var entities = await Module.ExecuteQueryAsync(new GetProblemAndThemeQuery(ProjectId));
            var entity = entities.FirstOrDefault(x => x.Tag == ExistTag);
            Assert.IsNull(entity);
        }

        [Test]
        public async Task RemoveProblemAndThemeCommandTests_WhenExistPlatformInDifferentProjects_IsSuccessful()
        {
            var otherProjectId = "project2";
            await CreateProject(projectId: otherProjectId);
            await Module.ExecuteCommandAsync(new SaveTagCommand(ExistTag, otherProjectId, true));
            await Module.ExecuteCommandAsync(new SavePlatformCommand(ExistTag, otherProjectId, ExistTag, ExistTag));
            await Module.ExecuteCommandAsync(new SaveProblemAndThemeCommand(otherProjectId, ExistTag, new ProblemAndTheme
            {
                Enabled = true,
                Subtypes = null,
                Platforms = new[] {ExistTag},
                Content = new LocalizeDictionary<ProblemAndThemeContent>()
                {
                    {"en", new ProblemAndThemeContent {Notification = "test", Title = "test"}}
                }
            }));

            await Module.ExecuteCommandAsync(new RemoveProblemAndThemeCommand(ExistTag, ProjectId));
            var entities = await Module.ExecuteQueryAsync(new GetProblemAndThemeQuery(otherProjectId));
            var entity = entities.FirstOrDefault(x => x.Tag == ExistTag);
            Assert.IsNotNull(entity);
        }

        [Test]
        public async Task RemoveProblemAndThemeCommandTests_WhenProjectNotExist_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new RemoveProblemAndThemeCommand(ExistTag, "not exist"));
            var entities = await Module.ExecuteQueryAsync(new GetProblemAndThemeQuery(ProjectId));
            var entity = entities.FirstOrDefault(x => x.Tag == ExistTag);
            Assert.IsNotNull(entity);
        }

        [Test]
        public async Task RemoveProblemAndThemeCommandTests_WhenTagNotExist_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new RemoveProblemAndThemeCommand("not exist", ProjectId));
            var entities = await Module.ExecuteQueryAsync(new GetProblemAndThemeQuery(ProjectId));
            var entity = entities.FirstOrDefault(x => x.Tag == ExistTag);
            Assert.IsNotNull(entity);
        }
    }
}
