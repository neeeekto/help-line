using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SavePlatform;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Queries.GetPlatforms;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTag;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.CreationOptions
{
    [NonParallelizable]
    [TestFixture]
    public class SavePlatformCommandTests : CreationOptionsTestBase
    {
        protected override string NS => nameof(SavePlatformCommandTests);
        public const string Key = "testKey";
        public const string Name = "testName";
        public const string Icon = "testIcon";

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        [Test]
        public async Task SavePlatformCommand_WhenDataIsValid_IsSuccessful()
        {
            var cmd = new SavePlatformCommand(Key, ProjectId, Name, Icon);
            await Module.ExecuteCommandAsync(cmd);
            var entities = await Module.ExecuteQueryAsync(new GetPlatformsQuery(ProjectId));
            Assert.AreEqual(1, entities.Count());

            var entity = entities.FirstOrDefault(x => x.Key == Key);
            Assert.IsNotNull(entity);
            Assert.AreEqual(Key, entity.Key);
            Assert.AreEqual(ProjectId, entity.ProjectId);
            Assert.AreEqual(Name ,entity.Name);
            Assert.AreEqual(Icon ,entity.Icon);
        }

        [Test]
        public async Task SavePlatformCommand_WhenDifferentProject_IsSuccessful()
        {
            var otherProjectId = "other";
            await CreateProject(projectId: otherProjectId);
            await Module.ExecuteCommandAsync(new SaveTagCommand(Key, otherProjectId, true));

            await Module.ExecuteCommandAsync(new SavePlatformCommand(Key, ProjectId, Name, Icon));
            await Module.ExecuteCommandAsync(new SavePlatformCommand(Key, otherProjectId, Name, Icon));
            var entities1 = await Module.ExecuteQueryAsync(new GetPlatformsQuery(ProjectId));
            Assert.AreEqual(1, entities1.Count());

            var entities2 = await Module.ExecuteQueryAsync(new GetPlatformsQuery(otherProjectId));
            Assert.AreEqual(1, entities2.Count());
        }

        [Test]
        public async Task SavePlatformCommand_WhenPlatformForTagExist_IsSuccessfulAndReplace()
        {
            var cmd = new SavePlatformCommand(Key, ProjectId, Name, Icon);
            await Module.ExecuteCommandAsync(cmd);
            await Module.ExecuteCommandAsync(cmd);
            var entities = await Module.ExecuteQueryAsync(new GetPlatformsQuery(ProjectId));
            Assert.AreEqual(1, entities.Count());
        }

        public class InvalidSource
        {
            public string ProjectId { get; set; } = CreationOptionsTestBase.ProjectId;
            public string Key { get; set; } = SavePlatformCommandTests.Key;
            public string Name { get; set; } = SavePlatformCommandTests.Name;

            public static IEnumerable<TestCaseData> Cases
            {
                get
                {
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = ""
                        }
                    ).SetName("Empty project: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = null
                        }
                    ).SetName("Empty project: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            ProjectId = "asd"
                        }
                    ).SetName("Not exist project");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Key = ""
                        }
                    ).SetName("Empty key: Empty");

                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Key = null
                        }
                    ).SetName("Empty key: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Key = "as"
                        }
                    ).SetName("Short key");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Name = null
                        }
                    ).SetName("Empty name: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Name = ""
                        }
                    ).SetName("Empty name");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Name = "as"
                        }
                    ).SetName("Short name");

                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task SavePlatformCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new SavePlatformCommand(src.Key, src.ProjectId, src.Name, Icon);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
