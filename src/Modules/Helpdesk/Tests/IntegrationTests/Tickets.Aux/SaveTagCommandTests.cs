using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTag;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTags;
using HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class SaveTagCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(SaveTagCommandTests);
        public const string TagKey = "test";

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        [Test]
        public async Task SaveTagCommand_WhenDataIsValid_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new SaveTagCommand(TagKey, ProjectId, true));
            var tags = await Module.ExecuteQueryAsync(new GetTagsQuery(ProjectId));
            var tag = tags.FirstOrDefault(x => x.Key == TagKey);
            Assert.IsNotNull(tag);
            Assert.IsTrue(tag.Enabled);
            Assert.AreEqual(ProjectId, tag.ProjectId);
        }

        [Test]
        public async Task SaveTagCommand_WhenTagIsDisabled_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new SaveTagCommand(TagKey, ProjectId, false));
            var tags = await Module.ExecuteQueryAsync(new GetTagsQuery(ProjectId, true));
            var tag = tags.FirstOrDefault(x => x.Key == TagKey);
            Assert.IsNull(tag);
        }
        [Test]
        public async Task SaveTagCommand_WhenTagIsExist_IsSuccessfulAndReplaceTag()
        {
            var cmd = new SaveTagCommand(TagKey, ProjectId, true);
            await Module.ExecuteCommandAsync(cmd);
            await Module.ExecuteCommandAsync(cmd);
            var entities = await Module.ExecuteQueryAsync(new GetTagsQuery(ProjectId));
            Assert.AreEqual(1, entities.Count());
        }

        [Test]
        public async Task SaveTagCommand_WhenDifferentProject_IsSuccessful()
        {
            var otherProjectId = "other";
            await CreateProject(projectId: otherProjectId);
            await Module.ExecuteCommandAsync(new SaveTagCommand(TagKey, ProjectId, true));
            await Module.ExecuteCommandAsync(new SaveTagCommand(TagKey, otherProjectId, true));
            var tags1 = await Module.ExecuteQueryAsync(new GetTagsQuery(ProjectId));
            Assert.AreEqual(1, tags1.Count());
            var tag1 = tags1.FirstOrDefault(x => x.Key == TagKey);
            Assert.IsNotNull(tag1);

            var tags2 = await Module.ExecuteQueryAsync(new GetTagsQuery(otherProjectId));
            Assert.AreEqual(1, tags2.Count());
            var tag2 = tags2.FirstOrDefault(x => x.Key == TagKey);
            Assert.IsNotNull(tag2);
        }

        public class InvalidSource
        {
            public string ProjectId { get; set; } = HelpdeskTestBase.ProjectId;
            public string Key { get; set; } = "test";

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
                            Key = ""
                        }
                    ).SetName("Empty key: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Key = null
                        }
                    ).SetName("Empty key: Null");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task SaveTagCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new SaveTagCommand(src.Key, src.ProjectId, false);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
