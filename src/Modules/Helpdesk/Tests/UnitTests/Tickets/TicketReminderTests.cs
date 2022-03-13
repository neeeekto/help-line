using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Domain.UnitTests.SeedWork;
using NSubstitute;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Domain.UnitTests.Tickets
{
    [TestFixture]
    public class TicketReminderTests : TicketTestsBase
    {
        /*
         * Cases:
         * Добавить любой +
         * Добавить финальный без резолва +
         * Добавить финальный с резолвом +
         * Добавить последовательность с финальным +
         * Отменить +
         */
        [Test]
        public async Task Where_AddFinalReminderWithoutResolve_Expect_Adding()
        {
            ClearServices();

            var ticket = await MakeTicket(new TestData());
            var reminder = new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("reminder"), false);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddReminderTicketCommand(reminder), ServiceProvider, initiator);

            var reminderEvt = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);
            Assert.AreEqual(ticket.Id, reminderEvt.AggregateId);
            Assert.AreEqual(initiator, reminderEvt.Initiator);
            Assert.AreEqual(reminder, reminderEvt.Reminder);
            Assert.Catch(() => AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket));
            await ServiceProvider.Scheduler.Received(1)
                .Schedule(Arg.Any<DateTime>(), Arg.Is(ticket.Id), Arg.Is(reminderEvt.ScheduleId));
        }

        [Test]
        public async Task When_RunFinalReminderWithoutResolve_Expect_ChangeStatusAndSendingMessage()
        {
            ClearServices();

            var ticket = await MakeTicket(new TestData());
            var reminder = new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("reminder"), false);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddReminderTicketCommand(reminder), ServiceProvider, initiator);

            var reminderEvt = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);
            await ticket.Execute(new RunTicketScheduleCommand(
                reminderEvt.ScheduleId), ServiceProvider, new SystemInitiator());

            var executedEvt = AssertAndGetPublishedEvent<TicketReminderExecutedEvent>(ticket);
            Assert.AreEqual(reminderEvt.Reminder.Id, executedEvt.ReminderId);

            var statusEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.AreEqual(TicketStatusKind.Opened, statusEvt.Status.Kind);
            Assert.AreEqual(TicketStatusType.Answered, statusEvt.Status.Type);

            var outputMessageEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.IsInstanceOf<OperatorInitiator>(outputMessageEvt.Initiator);
            Assert.AreEqual(new Message("reminder"), outputMessageEvt.Message);

            var timerEvents = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket);
            Assert.IsTrue(timerEvents.Any(x => x.LifeCycleType == TicketLifeCycleType.Resolving));
            Assert.AreEqual(1, timerEvents.Count());
        }

        [Test]
        public async Task When_RunFinalReminderWithResolve_Expect_SendMessageResolveAndPlannedLifecycle()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            var reminder = new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("reminder"), true);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddReminderTicketCommand(reminder), ServiceProvider, initiator);
            var reminderEvt = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);

            await ticket.Execute(new RunTicketScheduleCommand(
                reminderEvt.ScheduleId), ServiceProvider, new SystemInitiator());

            var outputMessageEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.AreEqual(new Message("reminder"), outputMessageEvt.Message);

            var statusEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.AreEqual(TicketStatusKind.Opened, statusEvt.Status.Kind);
            Assert.AreEqual(TicketStatusType.Resolved, statusEvt.Status.Type);

            var timerEvents = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket);
            Assert.IsTrue(timerEvents.Any(x => x.LifeCycleType == TicketLifeCycleType.Closing));
            Assert.IsTrue(timerEvents.Any(x => x.LifeCycleType == TicketLifeCycleType.Feedback));
            Assert.IsFalse(timerEvents.Any(x => x.LifeCycleType == TicketLifeCycleType.Resolving));
        }

        [Test]
        public async Task When_RunSequentialWithFinal_Expect_SendMessagePlannedSecondReminder()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            var secondReminder = new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("reminder2"), false);
            var firstReminder =
                new TicketSequentialReminder(new TimeSpan(1, 0, 0), new Message("reminder1"), secondReminder);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddReminderTicketCommand(firstReminder), ServiceProvider, initiator);

            var firstReminderScheduledEvt = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);
            ticket.ClearUncommittedEvents();
            await ticket.Execute(new RunTicketScheduleCommand(
                firstReminderScheduledEvt.ScheduleId), ServiceProvider, new SystemInitiator());

            var firstReminderExecutedEvt = AssertAndGetPublishedEvent<TicketReminderExecutedEvent>(ticket);
            Assert.AreEqual(firstReminder.Id, firstReminderExecutedEvt.ReminderId);

            var outputMessageEvt1 = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.AreEqual(new Message("reminder1"), outputMessageEvt1.Message);

            Assert.Throws<EventsNotPublishedException<TicketLifecyclePlannedEvent>>(() =>
                AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket));

            var firstReminderMessageAddedEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.AreEqual(firstReminder.Message, firstReminderMessageAddedEvt.Message);

            var firstReminderStatusChangedEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.AreEqual(TicketStatusKind.Opened, firstReminderStatusChangedEvt.Status.Kind);
            Assert.AreEqual(TicketStatusType.Answered, firstReminderStatusChangedEvt.Status.Type);

            var secondReminderScheduledEvt = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);
            ticket.ClearUncommittedEvents();
            await ticket.Execute(new RunTicketScheduleCommand(
                secondReminderScheduledEvt.ScheduleId), ServiceProvider, new SystemInitiator());

            var secondReminderExecutedEvt = AssertAndGetPublishedEvent<TicketReminderExecutedEvent>(ticket);
            Assert.AreEqual(secondReminder.Id, secondReminderExecutedEvt.ReminderId);

            var outputMessageEvt2 = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.AreEqual(new Message("reminder2"), outputMessageEvt2.Message);

            var secondReminderMessageAddedEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.AreEqual(secondReminder.Message, secondReminderMessageAddedEvt.Message);

            Assert.Throws<EventNotPublishedException<TicketStatusChangedEvent>>(() =>
                AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket));

            var timersEvt = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            Assert.AreEqual(TicketLifeCycleType.Resolving, timersEvt.LifeCycleType);
        }

        [Test]
        public async Task When_CancelFinalReminder_Expect_Canceling()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            var reminder = new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("reminder"), true);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddReminderTicketCommand(reminder), ServiceProvider, initiator);
            var reminderEvt = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new CancelReminderTicketCommand(reminder.Id), ServiceProvider, initiator);

            var cancelEvt = AssertAndGetPublishedEvent<TicketReminderCanceledEvent>(ticket);
            Assert.AreEqual(ticket.Id, cancelEvt.AggregateId);
            Assert.AreEqual(reminder.Id, cancelEvt.ReminderId);

            ServiceProvider.Scheduler.Received(1).Cancel(Arg.Is(reminderEvt.ScheduleId));
        }

        [Test]
        public async Task When_CancelReminderAfterCanceling_Expect_BreakWithTicketReminderShouldBeRule()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            var reminder = new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("reminder"), true);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddReminderTicketCommand(reminder), ServiceProvider, initiator);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new CancelReminderTicketCommand(reminder.Id), ServiceProvider, initiator);

            AssertBrokenRule<TicketReminderShouldBeInStatusRule>(() =>
                ticket.Execute(new CancelReminderTicketCommand(reminder.Id), ServiceProvider, initiator));
        }

        [Test]
        public async Task When_CancelReminderAfterExecuted_Expect_BreakWithTicketReminderShouldBeRule()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            var reminder = new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("reminder"), true);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddReminderTicketCommand(reminder), ServiceProvider, initiator);
            var reminderEvt = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);
            ticket.ClearUncommittedEvents();
            await ticket.Execute(new RunTicketScheduleCommand(
                reminderEvt.ScheduleId), ServiceProvider, new SystemInitiator());

            AssertBrokenRule<TicketReminderShouldBeInStatusRule>(() =>
                ticket.Execute(new CancelReminderTicketCommand(reminder.Id), ServiceProvider, initiator));
        }


        [Test]
        public async Task When_RunReminderIfUserHasNoEnabledChannels_Expect_CancelReminder()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var reminder2 = new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("reminder"), false);
            var reminder1 = new TicketSequentialReminder(new TimeSpan(1, 0, 0), new Message("reminder"), reminder2);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddReminderTicketCommand(reminder1), ServiceProvider, initiator);

            var reminderEvt = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);
            await ticket.Execute(new ToggleUserChannelTicketCommand(testData.UserId, false), ServiceProvider,
                initiator);
            ticket.ClearUncommittedEvents();
            await ticket.Execute(new RunTicketScheduleCommand(
                reminderEvt.ScheduleId), ServiceProvider, new SystemInitiator());

            var cancelEvt = AssertAndGetPublishedEvent<TicketReminderCanceledEvent>(ticket);
            Assert.AreEqual(reminderEvt.Reminder.Id, cancelEvt.ReminderId);
            Assert.Throws<EventsNotPublishedException<TicketReminderScheduledEvent>>(() =>
                AssertAndGetPublishedEvents<TicketReminderScheduledEvent>(ticket));
        }
    }
}
