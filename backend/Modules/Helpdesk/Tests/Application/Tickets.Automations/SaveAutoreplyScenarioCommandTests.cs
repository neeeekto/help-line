using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands.SaveAutoreplyScenario;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Queries.GetAutoreplyScenarios;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Automations
{
    [TestFixture]
    public class SaveAutoreplyScenarioCommandTests : TicketAutomationsTestBase
    {
        protected override string NS => nameof(SaveAutoreplyScenarioCommandTests);


        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
            await CreateOperator();
        }

        public AutoreplyScenarioAction MakeAction() => new AutoreplyScenarioAction
        {
            Message = new LocalizeDictionary<MessageDto>()
            {
                {
                    "en", new MessageDto()
                    {
                        Text = "test"
                    }
                }
            },
            Reminder = null,
            Resolve = false,
            Tags = new[] {Tag}
        };

        public AutoreplyScenarioCondition MakeCondition(TagCondition condition) => new AutoreplyScenarioCondition
        {
            Attachments = null,
            Keywords = null,
            Languages = new[] {"en"},
            TagConditions = new[] {condition}
        };

        public AutoreplyScenario MakeScenario(AutoreplyScenarioCondition condition, AutoreplyScenarioAction action) =>
            new AutoreplyScenario
            {
                Name = "test",
                Enabled = true,
                Weight = 1,
                ProjectId = ProjectId,
                Condition = condition,
                Action = action
            };

        [Test]
        public async Task SaveAutoreplyScenarioCommand_WhenDataIsValid_IsSuccessful()
        {
            var tagCond = MakeTagCondition();
            var data = MakeScenario(MakeCondition(tagCond), MakeAction());
            var result = await Module.ExecuteCommandAsync(new SaveAutoreplyScenarioCommand(data));

            var entities = await Module.ExecuteQueryAsync(new GetAutoreplyScenariosQuery(ProjectId));
            Assert.AreEqual(1, entities.Count());

            var entity = entities.FirstOrDefault(x => x.Id == result);
            Assert.IsNotNull(entity);
            Assert.AreEqual(data.Name, entity.Name);
            Assert.AreEqual(data.Enabled, entity.Enabled);
            Assert.AreEqual(data.Weight, entity.Weight);
            Assert.AreEqual(data.ProjectId, entity.ProjectId);
            Assert.AreEqual(data.Action.Resolve, entity.Action.Resolve);
            Assert.That(entity.Action.Reminder, Is.Null);
            Assert.That(entity.Action.Tags, Has.Exactly(1).EqualTo(Tag));

            Assert.AreEqual(data.Condition.Attachments, entity.Condition.Attachments);
            Assert.AreEqual(data.Condition.Keywords, entity.Condition.Keywords);

            var currentTagCond = entity.Condition.TagConditions.FirstOrDefault();
            Assert.AreEqual(tagCond.All, currentTagCond.All);
            Assert.AreEqual(tagCond.Include, currentTagCond.Include);
            Assert.That(currentTagCond.Tags, Has.Exactly(1).EqualTo(Tag));
        }

        [Test]
        public async Task SaveAutoreplyScenarioCommand_WhenExistAndValid_IsSuccessfulAndReplace()
        {
            var tagCond = MakeTagCondition();
            var data = MakeScenario(MakeCondition(tagCond), MakeAction());
            await Module.ExecuteCommandAsync(new SaveAutoreplyScenarioCommand(data));
            await Module.ExecuteCommandAsync(new SaveAutoreplyScenarioCommand(data));

            var entities = await Module.ExecuteQueryAsync(new GetAutoreplyScenariosQuery(ProjectId));
            Assert.AreEqual(1, entities.Count());
        }

        public class InvalidSource : AutoreplyScenario
        {
            public InvalidSource()
            {
                Name = "test";
                Enabled = true;
                Weight = 0;
                ProjectId = TicketAutomationsTestBase.ProjectId;
                Condition = new AutoreplyScenarioCondition()
                {
                    Attachments = null,
                    Keywords = null,
                    Languages = new[] {"en"},
                    TagConditions = new[]
                    {
                        new TagCondition
                        {
                            All = true,
                            Include = true,
                            Tags = new[] {Tag}
                        }
                    }
                };
                Action = new AutoreplyScenarioAction()
                {
                    Message = new BuildingBlocks.Application.LocalizeDictionary<MessageDto>()
                    {
                        {"en", new MessageDto() {Text = "test"}}
                    },
                    Reminder = null,
                    Resolve = false,
                    Tags = new[] {Tag}
                };
            }

            public InvalidSource ChangeCondition(Action<AutoreplyScenarioCondition> fn)
            {
                fn(Condition);
                return this;
            }

            public InvalidSource ChangeAction(Action<AutoreplyScenarioAction> fn)
            {
                fn(Action);
                return this;
            }

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
                            Name = ""
                        }
                    ).SetName("Empty name: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Name = null
                        }
                    ).SetName("Empty name: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Weight = -1
                        }
                    ).SetName("Negative weight");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeCondition((condition => { condition.Languages = null; }))
                    ).SetName("Condition, language: Null");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeCondition((condition => { condition.Languages = new string[] { }; }))
                    ).SetName("Condition, language: Empty");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeCondition((condition => { condition.Languages = new[] {""}; }))
                    ).SetName("Condition, language item: Empty");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeCondition((condition =>
                        {
                            condition.Languages = new string[] {null};
                        }))
                    ).SetName("Condition, language item: Null");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeCondition((condition => { condition.TagConditions = null; }))
                    ).SetName("Condition, TagConditions: Null");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeCondition((condition =>
                        {
                            condition.TagConditions = new TagCondition[] { };
                        }))
                    ).SetName("Condition, TagConditions: Empty");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeCondition((condition =>
                        {
                            condition.TagConditions = new TagCondition[] {null};
                        }))
                    ).SetName("Condition, TagConditions item: Null");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeCondition((condition =>
                        {
                            condition.TagConditions = new[]
                            {
                                new TagCondition()
                                {
                                    Tags = new string[] { }
                                }
                            };
                        }))
                    ).SetName("Condition, TagConditions item: Invalid");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeCondition((condition =>
                        {
                            condition.TagConditions = new[]
                            {
                                new TagCondition()
                                {
                                    Tags = new string[] {""}
                                }
                            };
                        }))
                    ).SetName("Condition, TagConditions item: Invalid");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeAction((action => { action.Tags = null; }))
                    ).SetName("Action, Tags: null");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeAction((action => { action.Tags = new string[] { }; }))
                    ).SetName("Action, Tags: Empty");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeAction((action => { action.Tags = new[] {""}; }))
                    ).SetName("Action, Tags: Empty item");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeAction((action => { action.Tags = new string[] {null}; }))
                    ).SetName("Action, Tags: Empty item");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeAction((action => { action.Message = null; }))
                    ).SetName("Action, Message: null");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeAction((action =>
                        {
                            action.Message = new LocalizeDictionary<MessageDto>();
                        }))
                    ).SetName("Action, Message: Empty");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeAction((action =>
                        {
                            action.Message = new LocalizeDictionary<MessageDto>()
                            {
                                {"", null}
                            };
                        }))
                    ).SetName("Action, Message: Empty items");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeAction((action =>
                        {
                            action.Message = new LocalizeDictionary<MessageDto>()
                            {
                                {"en", null}
                            };
                        }))
                    ).SetName("Action, Message: Empty items");
                    yield return new TestCaseData(
                        new InvalidSource().ChangeAction((action =>
                        {
                            action.Message = new LocalizeDictionary<MessageDto>()
                            {
                                {
                                    "en", new MessageDto()
                                    {
                                    }
                                }
                            };
                        }))
                    ).SetName("Action, Message: Invalid items");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task SaveAutoreplyScenarioCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new SaveAutoreplyScenarioCommand(src);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
