using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTag;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTagsDescription;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTagsDescriptions;
using HelpLine.Modules.Helpdesk.IntegrationTests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class SaveTagsDescriptionCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(SaveTagsDescriptionCommandTests);
        public const string ExistTag = "test";
        public static Guid Audience = Guid.NewGuid();

        public static readonly LocalizeDictionary<TagsDescriptionIssueContent> Content =
            new LocalizeDictionary<TagsDescriptionIssueContent>()
            {
                {
                    "en", new TagsDescriptionIssueContent
                    {
                        Text = "test"
                    }
                }
            };
        public static readonly IEnumerable<TagsDescriptionIssue> DescriptionIssues = new[]
        {
            new TagsDescriptionIssue
            {
                Audience = new[] {Audience},
                Contents = Content
            }
        };

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
            await Module.ExecuteCommandAsync(new SaveTagCommand(ExistTag, ProjectId, true));
        }

        [Test]
        public async Task SaveTagsDescriptionCommand_WhenDataIsValid_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new SaveTagsDescriptionCommand(ProjectId, ExistTag, true,
                DescriptionIssues));
            var entities = await Module.ExecuteQueryAsync(new GetTagsDescriptionsQuery(ProjectId));
            Assert.AreEqual(1, entities.Count());

            var entity = entities.FirstOrDefault(x => x.Tag == ExistTag);
            Assert.IsNotNull(entity);
            Assert.AreEqual(ExistTag, entity.Tag);
            Assert.AreEqual(ProjectId, entity.ProjectId);
            Assert.IsTrue(entity.Enabled);
            Assert.AreEqual(1, entity.Issues.Count());
            Assert.IsTrue(entity.Issues.First().Audience.Contains(Audience));
            Assert.IsTrue(entity.Issues.First().Contents.ContainsKey("en"));
            Assert.IsTrue(entity.Issues.First().Contents.TryGetValue("en", out var content));
            Assert.AreEqual("test", content.Text);
        }

        [Test]
        public async Task SaveTagsDescriptionCommand_WhenDifferentProject_IsSuccessful()
        {
            var otherProjectId = "other";
            await CreateProject(projectId: otherProjectId);
            await Module.ExecuteCommandAsync(new SaveTagCommand(ExistTag, otherProjectId, true));

            await Module.ExecuteCommandAsync(new SaveTagsDescriptionCommand(ProjectId, ExistTag, true,
                DescriptionIssues));
            await Module.ExecuteCommandAsync(new SaveTagsDescriptionCommand(otherProjectId, ExistTag, true,
                DescriptionIssues));
            var entities1 = await Module.ExecuteQueryAsync(new GetTagsDescriptionsQuery(ProjectId));
            Assert.AreEqual(1, entities1.Count());

            var entities2 = await Module.ExecuteQueryAsync(new GetTagsDescriptionsQuery(otherProjectId));
            Assert.AreEqual(1, entities2.Count());
        }

        [Test]
        public async Task SaveTagsDescriptionCommand_WhenDescriptionForTagExist_IsSuccessfulAndReplace()
        {
            var cmd = new SaveTagsDescriptionCommand(ProjectId, ExistTag, true, DescriptionIssues);
            await Module.ExecuteCommandAsync(cmd);
            await Module.ExecuteCommandAsync(cmd);
            var entities = await Module.ExecuteQueryAsync(new GetTagsDescriptionsQuery(ProjectId));
            Assert.AreEqual(1, entities.Count());
        }

        [Test]
        public async Task SaveTagsDescriptionCommand_WhenDisabled_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new SaveTagsDescriptionCommand(ProjectId, ExistTag, false,
                DescriptionIssues));
            var entities =
                await Module.ExecuteQueryAsync(
                    new GetTagsDescriptionsQuery(ProjectId, new Guid[] { }, new[] {ExistTag}));
            Assert.AreEqual(0, entities.Count());
        }

        [Test]
        public async Task SaveTagsDescriptionCommand_WhenHasAudience_IsSuccessful()
        {
            await Module.ExecuteCommandAsync(new SaveTagsDescriptionCommand(ProjectId, ExistTag, true,
                DescriptionIssues));
            var entities1 =
                await Module.ExecuteQueryAsync(
                    new GetTagsDescriptionsQuery(ProjectId, new [] { Audience }, new[] {ExistTag}));
            Assert.AreEqual(1, entities1.Count());
            Assert.IsTrue(entities1.Any(x => x.Tag == ExistTag));

            var entities2 =
                await Module.ExecuteQueryAsync(
                    new GetTagsDescriptionsQuery(ProjectId, new Guid[] { Guid.NewGuid() }, new[] {ExistTag}));
            Assert.AreEqual(0, entities2.Count());
        }

        public class InvalidSource
        {
            public string ProjectId { get; set; } = HelpdeskTestBase.ProjectId;
            public string Tag { get; set; } = ExistTag;
            public IEnumerable<TagsDescriptionIssue> Descriptions = DescriptionIssues;

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
                            Tag = ""
                        }
                    ).SetName("Empty tag: Empty");

                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Tag = null
                        }
                    ).SetName("Empty tag: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Tag = "asd"
                        }
                    ).SetName("Not exist tag");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Descriptions = null
                        }
                    ).SetName("Descriptions: null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Descriptions = new []
                            {
                                new TagsDescriptionIssue()
                                {
                                    Audience = null,
                                    Contents = Content
                                }
                            }
                        }
                    ).SetName("Descriptions Audience: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Descriptions = new []
                            {
                                new TagsDescriptionIssue()
                                {
                                    Audience = new Guid[] {},
                                    Contents = Content
                                }
                            }
                        }
                    ).SetName("Descriptions Audience: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Descriptions = new []
                            {
                                new TagsDescriptionIssue()
                                {
                                    Audience = new [] {Audience},
                                    Contents = null
                                }
                            }
                        }
                    ).SetName("Descriptions Contents: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Descriptions = new []
                            {
                                new TagsDescriptionIssue()
                                {
                                    Audience = new [] {Audience},
                                    Contents = new LocalizeDictionary<TagsDescriptionIssueContent>() {}
                                }
                            }
                        }
                    ).SetName("Descriptions Contents: Empty dictionary");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Descriptions = new []
                            {
                                new TagsDescriptionIssue()
                                {
                                    Audience = new [] {Audience},
                                    Contents = new LocalizeDictionary<TagsDescriptionIssueContent>()
                                    {
                                        {"en", new TagsDescriptionIssueContent() {}}
                                    }
                                }
                            }
                        }
                    ).SetName("Descriptions Contents: Empty message");


                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task SaveTagsDescriptionCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new SaveTagsDescriptionCommand(src.Tag, src.ProjectId, true, src.Descriptions);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
