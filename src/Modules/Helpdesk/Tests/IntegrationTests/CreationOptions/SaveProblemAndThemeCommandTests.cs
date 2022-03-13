using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SavePlatform;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Commands.SaveProblemAndTheme;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Queries.GetProblemAndTheme;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTag;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.CreationOptions
{
    [NonParallelizable]
    [TestFixture]
    public class SaveProblemAndThemeCommandTests : CreationOptionsTestBase
    {
        protected override string NS => nameof(SaveProblemAndThemeCommandTests);
        public const string ExistTag = "test";
        public const string ExistTag2 = "test2";
        public const string ExistTag3 = "test3";

        public readonly static ProblemAndTheme Entity = new ProblemAndTheme
        {
            Enabled = true,
            Subtypes = null,
            Tag = ExistTag,
            Platforms = new[] {ExistTag},
            Content = new LocalizeDictionary<ProblemAndThemeContent>()
            {
                {"en", new ProblemAndThemeContent {Notification = "test", Title = "test"}}
            }
        };

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
            await Module.ExecuteCommandAsync(new SaveTagCommand(ExistTag, ProjectId, true));
            await Module.ExecuteCommandAsync(new SaveTagCommand(ExistTag2, ProjectId, true));
            await Module.ExecuteCommandAsync(new SaveTagCommand(ExistTag3, ProjectId, true));
            await Module.ExecuteCommandAsync(new SavePlatformCommand(ExistTag, ProjectId,ExistTag, ExistTag));
        }

        [Test]
        public async Task SaveProblemAndThemeCommand_WhenDataIsValid_IsSuccessful()
        {
            var cmd = new SaveProblemAndThemeCommand(ProjectId, Entity.Tag, Entity);
            await Module.ExecuteCommandAsync(cmd);
            var entities = await Module.ExecuteQueryAsync(new GetProblemAndThemeQuery(ProjectId));
            Assert.AreEqual(1, entities.Count());

            var entity = entities.FirstOrDefault(x => x.Tag == ExistTag);
            Assert.IsNotNull(entity);
            Assert.AreEqual(ExistTag, entity.Tag);
            Assert.AreEqual(ProjectId, entity.ProjectId);
            Assert.AreEqual(Entity.Enabled, entity.Enabled);
            Assert.AreEqual(Entity.Subtypes, entity.Subtypes);
        }

        [Test]
        public async Task SaveProblemAndThemeCommand_WhenDisabled_IsSuccessful()
        {
            var cmd = new SaveProblemAndThemeCommand(ProjectId, ExistTag, new ProblemAndTheme
            {
                Enabled = true,
                Subtypes = new []
                {
                    new ProblemAndTheme
                    {
                        Enabled = true,
                        Subtypes = null,
                        Tag = ExistTag2,
                        Platforms = new[] {ExistTag},
                        Content = new LocalizeDictionary<ProblemAndThemeContent>()
                        {
                            {"en", new ProblemAndThemeContent {Notification = "test", Title = "test"}}
                        }
                    },
                    new ProblemAndTheme
                    {
                        Enabled = false,
                        Subtypes = null,
                        Tag = ExistTag3,
                        Platforms = new[] {ExistTag},
                        Content = new LocalizeDictionary<ProblemAndThemeContent>()
                        {
                            {"en", new ProblemAndThemeContent {Notification = "test", Title = "test"}}
                        }
                    }
                },
                Tag = ExistTag,
                Platforms = new[] {ExistTag},
                Content = new LocalizeDictionary<ProblemAndThemeContent>()
                {
                    {"en", new ProblemAndThemeContent {Notification = "test", Title = "test"}}
                },
            });
            await Module.ExecuteCommandAsync(cmd);
            var entities = await Module.ExecuteQueryAsync(new GetProblemAndThemeQuery(ProjectId, true));
            Assert.AreEqual(1, entities.Count());

            var subEntities = entities.FirstOrDefault(x => x.Tag == ExistTag).Subtypes;
            Assert.AreEqual(1, subEntities.Count());
        }

        [Test]
        public async Task SaveProblemAndThemeCommand_WhenDifferentProject_IsSuccessful()
        {
            var otherProjectId = "other";
            await CreateProject(projectId: otherProjectId);
            await Module.ExecuteCommandAsync(new SaveTagCommand(ExistTag, otherProjectId, true));
            await Module.ExecuteCommandAsync(new SavePlatformCommand(ExistTag, otherProjectId, ExistTag, ExistTag));

            await Module.ExecuteCommandAsync(new SaveProblemAndThemeCommand(ProjectId, Entity.Tag, Entity));
            await Module.ExecuteCommandAsync(new SaveProblemAndThemeCommand(otherProjectId,  Entity.Tag, Entity));
            var entities1 = await Module.ExecuteQueryAsync(new GetProblemAndThemeQuery(ProjectId));
            Assert.AreEqual(1, entities1.Count());

            var entities2 = await Module.ExecuteQueryAsync(new GetProblemAndThemeQuery(otherProjectId));
            Assert.AreEqual(1, entities2.Count());
        }

        [Test]
        public async Task SaveProblemAndThemeCommand_WhenForExistTag_IsSuccessfulAndReplace()
        {
            var cmd = new SaveProblemAndThemeCommand(ProjectId,  Entity.Tag, Entity);
            await Module.ExecuteCommandAsync(cmd);
            await Module.ExecuteCommandAsync(cmd);
            var entities = await Module.ExecuteQueryAsync(new GetProblemAndThemeQuery(ProjectId));
            Assert.AreEqual(1, entities.Count());
        }

        [Test]
        public async Task SaveProblemAndThemeCommand_WhenDeepEntity_IsSuccessful()
        {
            var cmd = new SaveProblemAndThemeCommand(ProjectId, ExistTag, new ProblemAndTheme
            {
                Enabled = true,
                Subtypes = new []
                {
                    new ProblemAndTheme
                    {
                        Enabled = true,
                        Subtypes = null,
                        Tag = ExistTag2,
                        Platforms = new[] {ExistTag},
                        Content = new LocalizeDictionary<ProblemAndThemeContent>()
                        {
                            {"en", new ProblemAndThemeContent {Notification = "test", Title = "test"}}
                        }
                    }
                },
                Tag = ExistTag,
                Platforms = new[] {ExistTag},
                Content = new LocalizeDictionary<ProblemAndThemeContent>()
                {
                    {"en", new ProblemAndThemeContent {Notification = "test", Title = "test"}}
                },
            });
            await Module.ExecuteCommandAsync(cmd);
            var entities = await Module.ExecuteQueryAsync(new GetProblemAndThemeQuery(ProjectId));
            Assert.AreEqual(1, entities.Count());

            var entity = entities.FirstOrDefault(x => x.Tag == ExistTag)?.Subtypes.FirstOrDefault();
            Assert.IsNotNull(entity);
            Assert.AreEqual(ExistTag2, entity.Tag);
            Assert.AreEqual(true, entity.Enabled);
            Assert.AreEqual(null, entity.Subtypes);
        }

        public class InvalidSource
        {
            public string ProjectId { get; set; } = CreationOptionsTestBase.ProjectId;
            public string Tag = ExistTag;
            public bool Enabled = true;
            public IEnumerable<string> PlatformsTag = new[] {ExistTag};

            public LocalizeDictionary<ProblemAndThemeContent> Content = new LocalizeDictionary<ProblemAndThemeContent>()
            {
                {"en", new ProblemAndThemeContent {Notification = "test", Title = "test"}}
            };

            public IEnumerable<ProblemAndTheme>? Subtypes = null;

            public ProblemAndTheme Entity => new ProblemAndTheme
            {
                Enabled = Enabled,
                Subtypes = Subtypes,
                Tag = Tag,
                Platforms = PlatformsTag,
                Content = Content
            };

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
                            PlatformsTag = null
                        }
                    ).SetName("Platforms tag: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            PlatformsTag = new string[] {}
                        }
                    ).SetName("Platforms tag: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            PlatformsTag = new string[] {null}
                        }
                    ).SetName("Platforms tag: Empty item");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            PlatformsTag = new [] {""}
                        }
                    ).SetName("Platforms tag: Empty item");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            PlatformsTag = new [] {"asd"}
                        }
                    ).SetName("Platforms tag: Not exist");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Content = null
                        }
                    ).SetName("Content: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Content = new LocalizeDictionary<ProblemAndThemeContent>() {}
                        }
                    ).SetName("Content: Empty dictionary");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Content = new LocalizeDictionary<ProblemAndThemeContent>() {{"en", new ProblemAndThemeContent() {}}}
                        }
                    ).SetName("Content: Empty items");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Subtypes = new []
                            {
                                new ProblemAndTheme()
                                {
                                    Enabled = true,
                                    Subtypes = null,
                                    Tag = ExistTag2,
                                    Platforms = new []{ExistTag},
                                    Content = null
                                }
                            }
                        }
                    ).SetName("Subtypes: Invalid");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Subtypes = new []
                            {
                                new ProblemAndTheme()
                                {
                                    Enabled = true,
                                    Subtypes = null,
                                    Tag = ExistTag2,
                                    Platforms = new []{"other"},
                                    Content = new LocalizeDictionary<ProblemAndThemeContent>()
                                    {
                                        {"en", new ProblemAndThemeContent()
                                        {
                                            Notification = "test",
                                            Title = "test"
                                        }}
                                    }
                                }
                            }
                        }
                    ).SetName("Subtypes: Has no parent platform");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Subtypes = new []
                            {
                                new ProblemAndTheme()
                                {
                                    Enabled = true,
                                    Subtypes = null,
                                    Tag = ExistTag,
                                    Platforms = new []{ExistTag},
                                    Content = new LocalizeDictionary<ProblemAndThemeContent>()
                                    {
                                        {"en", new ProblemAndThemeContent()
                                        {
                                            Notification = "test",
                                            Title = "test"
                                        }}
                                    }
                                }
                            }
                        }
                    ).SetName("Subtypes: Has parent tag");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Subtypes = new []
                            {
                                new ProblemAndTheme()
                                {
                                    Enabled = true,
                                    Subtypes = null,
                                    Tag = ExistTag2,
                                    Platforms = new []{ExistTag},
                                    Content = new LocalizeDictionary<ProblemAndThemeContent>()
                                    {
                                        {"en", new ProblemAndThemeContent()
                                        {
                                            Notification = "test",
                                            Title = "test"
                                        }}
                                    }
                                },
                                new ProblemAndTheme()
                                {
                                    Enabled = true,
                                    Subtypes = null,
                                    Tag = ExistTag2,
                                    Platforms = new []{ExistTag},
                                    Content = new LocalizeDictionary<ProblemAndThemeContent>()
                                    {
                                        {"en", new ProblemAndThemeContent()
                                        {
                                            Notification = "test",
                                            Title = "test"
                                        }}
                                    }
                                }
                            }
                        }
                    ).SetName("Subtypes: Has dublicate tag");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task SaveProblemAndThemeCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new SaveProblemAndThemeCommand(src.ProjectId, src.Entity.Tag, src.Entity);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }

        [Test]
        public async Task SaveProblemAndThemeCommand_WhenEntityIsNull_ThrowsInvalidCommandException()
        {
            var cmd = new SaveProblemAndThemeCommand(ProjectId, TestStr, null);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }

        [Test]
        public async Task SaveProblemAndThemeCommand_WhenTagIsNull_ThrowsInvalidCommandException()
        {
            var cmd = new SaveProblemAndThemeCommand(ProjectId, null, Entity);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
