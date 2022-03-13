using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketMessageTemplate;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketMessageTemplates;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class CreateTicketMessageTemplateCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(CreateTicketMessageTemplateCommandTests);

        protected Dictionary<string, TicketMessageTemplateContent?> MakeContent() =>
            new Dictionary<string, TicketMessageTemplateContent?>()
            {
                {EngLangKey, new TicketMessageTemplateContent()
                {
                    Message = new MessageDto{Text = "test"},
                    Title = "test"
                }}
            };

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        [Test]
        public async Task CreateTicketMessageTemplateCommand_WhenIsValid_IsSuccessful()
        {
            var content = MakeContent();
            var group = "group";
            var templateId = await Module.ExecuteCommandAsync(new CreateTicketMessageTemplateCommand(ProjectId, content, group));

            var templates = await Module.ExecuteQueryAsync(new GetTicketMessageTemplatesQuery(ProjectId));
            var template = templates.FirstOrDefault(x => x.Id == templateId);

            Assert.That(template, Is.Not.Null);
            Assert.That(template.Id, Is.EqualTo(templateId));
            Assert.That(template.Group, Is.EqualTo(group));
            Assert.That(template.Order, Is.EqualTo(templates.Count() - 1));
            Assert.That(template.ModifyDate, Is.LessThanOrEqualTo(DateTime.UtcNow));
            Assert.That(template.ProjectId, Is.EqualTo(ProjectId));
            Assert.That(template.Content.ContainsKey("en"), Is.True);
            Assert.That(template.Content.Values.FirstOrDefault()?.Message.Text, Is.EqualTo("test"));
            Assert.That(template.Content.Values.FirstOrDefault()?.Title, Is.EqualTo("test"));
        }

        [Test]
        public async Task CreateTicketMessageTemplateCommand_WhenIsValid_HasCorrectOrder()
        {
            var content = MakeContent();
            var group = "group";
            var templateId1 = await Module.ExecuteCommandAsync(new CreateTicketMessageTemplateCommand(ProjectId, content, group));
            var templateId2 = await Module.ExecuteCommandAsync(new CreateTicketMessageTemplateCommand(ProjectId, content, group));

            var templates = await Module.ExecuteQueryAsync(new GetTicketMessageTemplatesQuery(ProjectId));
            var template = templates.FirstOrDefault(x => x.Id == templateId2);

            Assert.That(template.Order, Is.EqualTo(templates.Count() - 1));
        }

        public class InvalidSource
        {
            public Dictionary<string, TicketMessageTemplateContent?> Contents =
                new Dictionary<string, TicketMessageTemplateContent?>()
                {
                    {
                        EngLangKey, new TicketMessageTemplateContent()
                        {
                            Message = new MessageDto{Text = "test"},
                            Title = "test"
                        }
                    }
                };

            public string ProjectId = TicketAuxTestBase.ProjectId;
            public string? Group = null;
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
                            Contents = null
                        }
                    ).SetName("Contents: null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Contents = new Dictionary<string, TicketMessageTemplateContent?>() {}
                        }
                    ).SetName("Contents: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Contents = new Dictionary<string, TicketMessageTemplateContent?>()
                            {
                                {"", new TicketMessageTemplateContent()
                                {
                                    Message = new MessageDto{ Text = "test"},
                                    Title = "test"
                                }}
                            }
                        }
                    ).SetName("Contents: Invalid key");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Contents = new Dictionary<string, TicketMessageTemplateContent?>()
                            {
                                {"", new TicketMessageTemplateContent()
                                {
                                    Message = new MessageDto{ Text = "test"},
                                    Title = ""
                                }}
                            }
                        }
                    ).SetName("Contents: Invalid title, empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Contents = new Dictionary<string, TicketMessageTemplateContent?>()
                            {
                                {"", new TicketMessageTemplateContent()
                                {
                                    Message = new MessageDto{ Text = "test"},
                                    Title = null
                                }}
                            }
                        }
                    ).SetName("Contents: Invalid title, null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Contents = new Dictionary<string, TicketMessageTemplateContent?>()
                            {
                                {"", new TicketMessageTemplateContent()
                                {
                                    Message = new MessageDto{ Text = ""},
                                    Title = null
                                }}
                            }
                        }
                    ).SetName("Contents: Invalid message");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task CreateTicketMessageTemplateCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new CreateTicketMessageTemplateCommand(src.ProjectId, src.Contents, src.Group);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
