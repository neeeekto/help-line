using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Commands.SaveAutoreplyScenario;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Queries.GetTicket;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Application.Tickets.Automations
{
    [TestFixture]
    public class AutoreplyRepositoryTests : TicketAutomationsTestBase
    {
        protected override string NS => nameof(SaveAutoreplyScenarioCommandTests);

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

        [SetUp]
        public async Task Setup()
        {
            await CreateProject();
            await CreateOperator();
        }

        private async Task<TicketView> CreateScenarioAndTicket(AutoreplyScenario scenario,
            TicketTestData ticketTestData)
        {
            await Module.ExecuteCommandAsync(new SaveAutoreplyScenarioCommand(scenario));
            var ticketId = await CreateTicket(ticketTestData);
            await BusServiceFactory.EmitAll();
            await BusServiceFactory.EmitAll();
            return await Module.ExecuteQueryAsync(new GetTicketQuery(ticketId))!;
        }

        [Test]
        public async Task When_ScenarioHasNoMessage_Expect_NoAction()
        {
            var tagCond = MakeTagCondition();
            var action = MakeAction();
            action.Message = new LocalizeDictionary<MessageDto>() { };
            var scenario = MakeScenario(MakeCondition(tagCond), action);
            var ticketData = new TicketTestData();
            ticketData.Tags = new[] {Tag};
            var ticketView = await CreateScenarioAndTicket(scenario, ticketData);
            Assert.That(ticketView.Events.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task When_ScenarioActionHasMessage_Expect_SendMessage()
        {
            var tagCond = MakeTagCondition();
            var action = MakeAction();
            var scenario = MakeScenario(MakeCondition(tagCond), action);
            var ticketData = new TicketTestData();
            ticketData.Tags = new[] {Tag};
            var ticketView = await CreateScenarioAndTicket(scenario, ticketData);
            Assert.That(ticketView.Events.OfType<TicketOutgoingMessageEventView>().Any(), Is.True);
        }

        [Test]
        public async Task When_ScenarioActionWithResolve_Expect_Resolve()
        {
            var tagCond = MakeTagCondition();
            var action = MakeAction();
            action.Resolve = true;
            var scenario = MakeScenario(MakeCondition(tagCond), action);
            var ticketData = new TicketTestData();
            ticketData.Tags = new[] {Tag};
            var ticketView = await CreateScenarioAndTicket(scenario, ticketData);
            Assert.That(ticketView.Status.Type, Is.EqualTo(TicketStatusType.Resolved));
        }

        [Test]
        public async Task When_ScenarioActionWithTags_Expect_AddedTags()
        {
            var tagCond = MakeTagCondition();
            var action = MakeAction();
            var tag1 = "tag1";
            var tag2 = "tag2";
            action.Tags = new[] {tag1, tag2};
            var scenario = MakeScenario(MakeCondition(tagCond), action);
            var ticketData = new TicketTestData();
            ticketData.Tags = new[] {Tag};
            var ticketView = await CreateScenarioAndTicket(scenario, ticketData);
            Assert.That(ticketView.Tags, Has.Exactly(1).EqualTo(tag1).And.Exactly(1).EqualTo(tag2));
        }

        [Test]
        public async Task When_ScenarioActionWithReminder_Expect_AddedReminder()
        {
            var tagCond = MakeTagCondition();
            var action = MakeAction();
            action.Reminder = new TicketFinalReminderDto()
            {
                Delay = new TimeSpan(TimeSpan.TicksPerDay),
                Message = new MessageDto
                {
                    Text = "test"
                }
            };
            var scenario = MakeScenario(MakeCondition(tagCond), action);
            var ticketData = new TicketTestData();
            ticketData.Tags = new[] {Tag};
            var ticketView = await CreateScenarioAndTicket(scenario, ticketData);
            Assert.That(ticketView.Events.OfType<TicketReminderEventView>().Any(), Is.True);
        }

        // todo: condiion tests!!!

        [Test]
        public async Task When_ScenarioConditionByHasAttachments_Expect_Success()
        {
            var tagCond = MakeTagCondition();
            var cond = MakeCondition(tagCond);
            cond.Attachments = true;
            var action = MakeAction();
            var scenario = MakeScenario(cond, action);
            var ticketData = new TicketTestData();
            ticketData.Tags = new[] {Tag};
            ticketData.Message = new MessageDto()
            {
                Text = "test",
                Attachments = new[] {"test"}
            };
            var ticketView = await CreateScenarioAndTicket(scenario, ticketData);
            Assert.That(ticketView.Events.OfType<TicketOutgoingMessageEventView>().Any(), Is.True);
        }

        [Test]
        public async Task When_ScenarioConditionByHasNoAttachments_Expect_Success()
        {
            var tagCond = MakeTagCondition();
            var cond = MakeCondition(tagCond);
            cond.Attachments = false;
            var action = MakeAction();
            var scenario = MakeScenario(cond, action);
            var ticketData = new TicketTestData();
            ticketData.Tags = new[] {Tag};
            ticketData.Message = new MessageDto()
            {
                Text = "test",
            };
            var ticketView = await CreateScenarioAndTicket(scenario, ticketData);
            Assert.That(ticketView.Events.OfType<TicketOutgoingMessageEventView>().Any(), Is.True);
        }

        [Test]
        public async Task When_ScenarioConditionByLanguages_Expect_NoActions()
        {
            var tagCond = MakeTagCondition();
            var cond = MakeCondition(tagCond);
            cond.Languages = new [] {"ru"};
            var action = MakeAction();
            var scenario = MakeScenario(cond, action);
            var ticketData = new TicketTestData();
            ticketData.Tags = new[] {Tag};
            ticketData.Message = new MessageDto()
            {
                Text = "test",
            };
            var ticketView = await CreateScenarioAndTicket(scenario, ticketData);
            Assert.That(ticketView.Events.OfType<TicketOutgoingMessageEventView>().Any(), Is.False);
        }

        [Test]
        public async Task When_ScenarioConditionByKeyword_Expect_Success()
        {
            var tagCond = MakeTagCondition();
            var cond = MakeCondition(tagCond);
            cond.Keywords = new LocalizeDictionary<string>() {{"en", $"\"test\""}};
            var action = MakeAction();
            var scenario = MakeScenario(cond, action);
            var ticketData = new TicketTestData();
            ticketData.Tags = new[] {Tag};
            ticketData.Message = new MessageDto()
            {
                Text = "test message test message",
            };
            var ticketView = await CreateScenarioAndTicket(scenario, ticketData);
            Assert.That(ticketView.Events.OfType<TicketOutgoingMessageEventView>().Any(), Is.True);
        }

        [Test]
        public async Task When_ScenarioConditionComposite_Expect_Actions()
        {
            var tagCond = MakeTagCondition();
            var cond = MakeCondition(tagCond);
            cond.Keywords = new LocalizeDictionary<string>() {{"en", $"\"test\""}};
            cond.Languages = new [] {"en"};
            cond.Attachments = true;
            var action = MakeAction();
            var scenario = MakeScenario(cond, action);
            var ticketData = new TicketTestData();
            ticketData.Tags = new[] {Tag};
            ticketData.Message = new MessageDto()
            {
                Text = "test message test message",
                Attachments = new []{"test"}
            };
            var ticketView = await CreateScenarioAndTicket(scenario, ticketData);
            Assert.That(ticketView.Events.OfType<TicketOutgoingMessageEventView>().Any(), Is.True);
        }

    }
}
