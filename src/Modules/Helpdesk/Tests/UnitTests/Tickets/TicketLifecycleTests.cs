using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using NSubstitute;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Domain.UnitTests.Tickets
{
    [TestFixture]
    public class TicketLifecycleTests : TicketTestsBase
    {
        /*
         * Cases:
         * Резолв таймер на активном тикете - успех - нет событий +
         * Резолв таймер на неактивном тикете - успех +
         * Feedback таймер - успех +
         * Close таймер на активном - успех - нет событий +
         * Close таймер на неактивном - успех - закрытие +
         * Ответ игрока после резолва - сбросить все таймеры и вернуться на awaitReply +
         * Срабатывание таймера Close раньше фидбека - ошибка последовательности ???????????????
         * Срабатывание неизвестного таймера
         */
        [Test]
        public async Task ResolveTimerOnOpenAndActiveTicket()
        {
            ClearServices();
            var delay = new TimeSpan(500, 0, 0);

            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var message = new Message("Test");
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                message), ServiceProvider, new OperatorInitiator(new OperatorId(Guid.NewGuid())));

            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            ticket.ClearUncommittedEvents();


            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());

            var timerProlEvt = AssertAndGetPublishedEvent<TicketLifecycleProlongatedEvent>(ticket);
            Assert.AreEqual(resolveTimer.ScheduleId, timerProlEvt.ScheduleId);
            Assert.AreEqual(ticket.Id, timerProlEvt.AggregateId);
            Assert.GreaterOrEqual(DateTime.UtcNow.Add(delay), timerProlEvt.NextDate);
            ServiceProvider.Scheduler.Received(1)
                .Prolong(Arg.Any<DateTime>(), Arg.Is(resolveTimer.AggregateId), Arg.Is(resolveTimer.ScheduleId));
        }

        [Test]
        public async Task When_ResolveTimerRunOnOpenAndNotActiveTicket_Expect_Resolve()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var message = new Message("Test");
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                message), ServiceProvider, new OperatorInitiator(new OperatorId(Guid.NewGuid())));

            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());

            var statusChangedEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.AreEqual(TicketStatusType.Resolved, statusChangedEvt.Status.Type);
            Assert.AreEqual(TicketStatusKind.Opened, statusChangedEvt.Status.Kind);

            var timerEvents = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket);
            Assert.IsTrue(timerEvents.Any(x => x.LifeCycleType == TicketLifeCycleType.Closing));
            Assert.IsTrue(timerEvents.Any(x => x.LifeCycleType == TicketLifeCycleType.Feedback));
        }

        [Test]
        public async Task When_ReRunResolveTimer_Expect_NoEvents()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var message = new Message("Test");
            var ticket = await MakeTicket(new TestData());
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                message), ServiceProvider, new OperatorInitiator(new OperatorId(Guid.NewGuid())));

            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();
            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());

            AssertEventsNotPublished<TicketLifecycleExecutedEvent>(ticket);
            AssertEventsNotPublished<TicketLifecycleCanceledEvent>(ticket);
            AssertEventsNotPublished<TicketStatusChangedEvent>(ticket);
        }

        [Test]
        public async Task When_RunFeedbackTimer_Expect_SendFeeback()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var message = new Message("Test");
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                message), ServiceProvider, new OperatorInitiator(new OperatorId(Guid.NewGuid())));
            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());
            var feedbackTimers = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket)
                .First(x => x.LifeCycleType == TicketLifeCycleType.Feedback);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(
                feedbackTimers.ScheduleId), ServiceProvider, new SystemInitiator());

            AssertEventsPublished<TicketFeedbackSentEvent>(ticket);
            ServiceProvider.FeedbackDispatcher.Received(1).Enqueue(Arg.Is(ticket.Id), Arg.Any<TicketFeedbackId>(),
                Arg.Any<IEnumerable<UserChannel>>(), Arg.Is(testData.ProjectId));
        }

        [Test]
        public async Task When_ReRunFeedbackTimer_Expect_NoEvents()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var message = new Message("Test");
            var ticket = await MakeTicket(new TestData());
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                message), ServiceProvider, new OperatorInitiator(new OperatorId(Guid.NewGuid())));
            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());
            var feedbackTimers = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket)
                .First(x => x.LifeCycleType == TicketLifeCycleType.Feedback);
            await ticket.Execute(new RunTicketScheduleCommand(
                feedbackTimers.ScheduleId), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();


            await ticket.Execute(new RunTicketScheduleCommand(
                feedbackTimers.ScheduleId), ServiceProvider, new SystemInitiator());
            AssertEventsNotPublished<TicketFeedbackSentEvent>(ticket);
            AssertEventsNotPublished<TicketLifecycleCanceledEvent>(ticket);
            AssertEventsNotPublished<TicketLifecycleExecutedEvent>(ticket);
        }

        [Test]
        public async Task When_RunCloseTimerOnActiveTicket_Expect_ProlongatedClose()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var message = new Message("Test");
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                message), ServiceProvider, new OperatorInitiator(new OperatorId(Guid.NewGuid())));

            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());

            var closeEvent = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket)
                .First(x => x.LifeCycleType == TicketLifeCycleType.Closing);
            ticket.ClearUncommittedEvents();

            delay = new TimeSpan(500, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));

            await ticket.Execute(new RunTicketScheduleCommand(
                closeEvent.ScheduleId), ServiceProvider, new SystemInitiator());

            var evt = AssertAndGetPublishedEvent<TicketLifecycleProlongatedEvent>(ticket);
            Assert.IsTrue(evt.LifeCycleType == TicketLifeCycleType.Closing);
            Assert.IsTrue(evt.ScheduleId == closeEvent.ScheduleId);
        }

        [Test]
        public async Task When_RunCloseTimerOnNotActiveTicketBeforeFeedback_Expect_CloseTicketAndCancelFeedback()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var message = new Message("Test");
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                message), ServiceProvider, new OperatorInitiator(new OperatorId(Guid.NewGuid())));

            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());

            var closeEvent = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket)
                .First(x => x.LifeCycleType == TicketLifeCycleType.Closing);
            var feedbackEvent = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket)
                .First(x => x.LifeCycleType == TicketLifeCycleType.Feedback);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(
                closeEvent.ScheduleId), ServiceProvider, new SystemInitiator());

            var evt = AssertAndGetPublishedEvent<TicketLifecycleExecutedEvent>(ticket);
            Assert.IsTrue(evt.LifeCycleType == TicketLifeCycleType.Closing);
            Assert.IsTrue(evt.ScheduleId == closeEvent.ScheduleId);

            var cancelEvt = AssertAndGetPublishedEvent<TicketLifecycleCanceledEvent>(ticket);
            Assert.AreEqual(TicketLifeCycleType.Feedback, cancelEvt.LifeCycleType);
            Assert.AreEqual(feedbackEvent.ScheduleId, cancelEvt.ScheduleId);

            var closeEvt = AssertAndGetPublishedEvent<TicketClosedEvent>(ticket);
            Assert.AreEqual(ticket.Id, closeEvt.AggregateId);
        }

        [Test]
        [Obsolete("Check: Move to Discussion or remove")]
        public async Task When_UserReplyAfterOperatorReplyBeforeResolve_Expect_ChangeStatusToAwaitReplyAndCancelTimers()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var message = new Message("Test");
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                message), ServiceProvider, new OperatorInitiator(new OperatorId(Guid.NewGuid())));

            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddIncomingMessageTicketCommand(message, testData.UserId, testData.Channel),
                ServiceProvider, new SystemInitiator());

            var cancelLcEvt = AssertAndGetPublishedEvents<TicketLifecycleCanceledEvent>(ticket);
            Assert.AreEqual(2, cancelLcEvt.Count());
            Assert.IsTrue(cancelLcEvt.Any(x => x.LifeCycleType == TicketLifeCycleType.Closing));
            Assert.IsTrue(cancelLcEvt.Any(x => x.LifeCycleType == TicketLifeCycleType.Feedback));

            var statusEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.IsTrue(statusEvt.Status.In(TicketStatusType.AwaitingReply));
        }
    }
}
