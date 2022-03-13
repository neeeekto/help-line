using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketReminder;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketReminder;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketReminders;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.IntegrationTests.Tickets.Aux
{
    [NonParallelizable]
    [TestFixture]
    public class RemoveTicketReminderCommandTests : TicketAuxTestBase
    {
        protected override string NS => nameof(RemoveFromUnsubscribedCommandTests);


        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
        }

        private Task<Guid> CreateReminder()
        {
            return Module.ExecuteCommandAsync(new CreateTicketReminderCommand(ProjectId,
                new TicketReminderData()
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
                            {EngLangKey, new MessageDto()
                            {
                                Text = "test"
                            }}
                        },
                        Resolve = false
                    }
                }));
        }

        [Test]
        public async Task RemoveFromUnsubscribedCommand_WhenIsValid_IsSuccessful()
        {
            var reminderId = await CreateReminder();
            await Module.ExecuteCommandAsync(new RemoveTicketReminderCommand(reminderId));

            var entities = await Module.ExecuteQueryAsync(new GetTicketRemindersQuery(ProjectId));
            Assert.That(entities, Is.Empty);
        }
    }
}
