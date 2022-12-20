using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketReminder;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketReminders;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Aux
{
    [TestFixture]
    public class CreateTicketReminderCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(CreateTicketReminderCommandTests);

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        private TicketReminderData MakeData() => new TicketReminderData()
        {
            Description = TestStr,
            Enabled = false,
            Group = TestStr,
            Name = TestStr,
            Reminder = new TicketFinalReminderItem()
            {
                Delay = new TimeSpan(Int32.MaxValue),
                Message = new LocalizeDictionary<MessageDto>()
                {
                    {
                        EngLangKey,
                        new MessageDto()
                        {
                            Text = "test"
                        }
                    }
                },
                Resolve = false
            }
        };

        [Test]
        public async Task CreateTicketReminderCommand_WhenIsValid_IsSuccessful()
        {
            var data = MakeData();
            var entityId = await Module.ExecuteCommandAsync(new CreateTicketReminderCommand(ProjectId, data));

            var entities = await Module.ExecuteQueryAsync(new GetTicketRemindersQuery(ProjectId));
            var entity = entities.FirstOrDefault(x => x.Id == entityId);

            Assert.That(entity, Is.Not.Null);
            Assert.That(entity.Id, Is.EqualTo(entityId));
            Assert.That(entity.Group, Is.EqualTo(data.Group));
            Assert.That(entity.Description, Is.EqualTo(data.Description));
            Assert.That(entity.Enabled, Is.EqualTo(data.Enabled));
            Assert.That(entity.ProjectId, Is.EqualTo(ProjectId));
            Assert.That(entity.Reminder.Message.ContainsKey(EngLangKey), Is.True);
        }

        public class InvalidSource : TicketReminderData
        {
            public string ProjectId = HelpdeskTestBase.ProjectId;
            public InvalidSource()
            {
                Description = TestStr;
                Enabled = false;
                Group = TestStr;
                Name = TestStr;
                Reminder = new TicketFinalReminderItem()
                {
                    Delay = new TimeSpan(Int32.MaxValue),
                    Message = new LocalizeDictionary<MessageDto>()
                    {
                        {
                            EngLangKey,
                            new MessageDto()
                            {
                                Text = "test"
                            }
                        }
                    },
                    Resolve = false
                };
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
                    ).SetName("Invalid name: Empty");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Name = null
                        }
                    ).SetName("Invalid name: Null");
                    yield return new TestCaseData(
                        new InvalidSource
                        {
                            Reminder = null
                        }
                    ).SetName("Invalid reminders: null");
                }
            }
        }

        [TestCaseSource(typeof(InvalidSource), nameof(InvalidSource.Cases))]
        public async Task CreateTicketReminderCommand_WhenDataIsInvalid_ThrowsInvalidCommandException(
            InvalidSource src)
        {
            var cmd = new CreateTicketReminderCommand(src.ProjectId, src);
            Assert.CatchAsync<InvalidCommandException>(async () => { await Module.ExecuteCommandAsync(cmd); });
        }
    }
}
