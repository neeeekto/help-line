using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using NSubstitute;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Domain.Tickets
{
    [TestFixture]
    public class TicketStatusesTests : TicketTestsBase
    {
        [Test]
        public async Task AllMethods_WithoutInitiator_BreakWithInitiatorMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new ResolveTicketCommand(), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(async () =>
                await ticket.Execute(new RejectTicketCommand(), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(async () =>
                await ticket.Execute(new ReopenTicketCommand(), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(async () =>
                await ticket.Execute(new ApproveRejectTicketCommand(new TicketAuditId()), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(async () =>
                await ticket.Execute(new CancelRejectTicketCommand(new TicketAuditId()), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(async () =>
                await ticket.Execute(new DenyRejectTicketCommand(new TicketAuditId(),
                    new Message("Deny")), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(async () =>
                await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, null!));
        }

        [Test]
        public async Task AllMethods_ClosedTicket_BreakTicketShouldNotBeClosedRule()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var ticket = await MakeTicket(new TestData());
            await ticket.Execute(new RejectTicketCommand(), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketShouldNotBeClosedRule>(async () =>
                await ticket.Execute(new RejectTicketCommand(), ServiceProvider, new SystemInitiator()));
            AssertBrokenRule<TicketShouldNotBeClosedRule>(async () =>
                await ticket.Execute(new ReopenTicketCommand(), ServiceProvider, new SystemInitiator()));
            AssertBrokenRule<TicketShouldNotBeClosedRule>(async () =>
                await ticket.Execute(new ResolveTicketCommand(), ServiceProvider, new SystemInitiator()));
            AssertBrokenRule<TicketShouldNotBeClosedRule>(async () =>
                await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator()));
            AssertBrokenRule<TicketShouldNotBeClosedRule>(async () =>
                await ticket.Execute(new ApproveRejectTicketCommand(new TicketAuditId()), ServiceProvider,
                    new SystemInitiator()));
            AssertBrokenRule<TicketShouldNotBeClosedRule>(async () =>
                await ticket.Execute(new CancelRejectTicketCommand(new TicketAuditId()), ServiceProvider,
                    new SystemInitiator()));
            AssertBrokenRule<TicketShouldNotBeClosedRule>(async () =>
                await ticket.Execute(new DenyRejectTicketCommand(new TicketAuditId(), new Message("aaa")
                ), ServiceProvider, new SystemInitiator()));
        }


        [Test]
        public async Task When_ReopenNewTicket_Expect_BreakTicketCheckStatusRule()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketCheckStatusRule>(async () =>
                await ticket.Execute(new ReopenTicketCommand(), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task When_ReopenResolveAnsweredTicket_Expect_SetAnsweredStatusCanceledCloseAndPlannedResolve()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("Test")), ServiceProvider, new SystemInitiator());
            var lcEvts = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket);
            var resolveLcEvt = lcEvts.First(x => x.LifeCycleType == TicketLifeCycleType.Resolving);

            await ticket.Execute(new RunTicketScheduleCommand(
                resolveLcEvt.ScheduleId), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new ReopenTicketCommand(), ServiceProvider, new SystemInitiator());
            var statusEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.That(statusEvt.Status, Is.EqualTo(TicketStatus.Opened(TicketStatusType.Answered)));

            var lcEvtsCanceled = AssertAndGetPublishedEvents<TicketLifecycleCanceledEvent>(ticket);
            Assert.IsTrue(lcEvtsCanceled.Any(x => x.LifeCycleType == TicketLifeCycleType.Closing));
            Assert.IsTrue(lcEvtsCanceled.Any(x => x.LifeCycleType == TicketLifeCycleType.Feedback));

            var lcEvtsPlanned = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket);
            Assert.IsTrue(lcEvtsPlanned.Any(x => x.LifeCycleType == TicketLifeCycleType.Resolving));
        }


        #region Pending

        [Test]
        public async Task When_SetPendingOnNewTicket_Expect_PendingState()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator());

            var evt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);

            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(TicketStatus.Pending(TicketStatusType.New), evt.Status);
            Assert.IsInstanceOf<SystemInitiator>(evt.Initiator);

            AssertEventsNotPublished<TicketLifecyclePlannedEvent>(ticket);
        }

        [Test]
        public async Task When_RemovePendingOnNewTicket_Expect_NoEvents()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new TogglePendingTicketCommand(false), ServiceProvider, new SystemInitiator());

            AssertEventsNotPublished<TicketStatusChangedEvent>(ticket);
            AssertEventsNotPublished<TicketLifecyclePlannedEvent>(ticket);
        }

        [Test]
        public async Task When_SetPendingOnPendingTicket_Expect_NoEvents()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator());
            AssertEventsNotPublished<TicketStatusChangedEvent>(ticket);
        }


        [Test]
        public async Task When_RemovePendingOnNotPendingTicket_Expect_NoEvents()
        {
            await When_RemovePendingOnNewTicket_Expect_NoEvents();
        }

        [Test]
        public async Task
            When_SetPendingOnTicketWithOneResolveReminder_Expect_BrokeTicketMustHaveNotResolveReminderRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);

            await ticket.Execute(
                new AddReminderTicketCommand(
                    new TicketFinalReminder(new TimeSpan(1, 1, 1), testData.Message, true)),
                ServiceProvider,
                new SystemInitiator());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketMustHaveNotResolveReminderRule>(async () =>
                await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task
            When_SetPendingOnTicketWithResolveReminderInSequence_Expect_BrokeTicketMustHaveNotResolveReminderRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(
                new AddReminderTicketCommand(
                    new TicketSequentialReminder(new TimeSpan(1, 1, 1), testData.Message,
                        new TicketFinalReminder(new TimeSpan(1, 1, 1), testData.Message, true))),
                ServiceProvider,
                new SystemInitiator()
            );
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketMustHaveNotResolveReminderRule>(async () =>
                await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator()));
        }


        [Test]
        public async Task When_SetPendingOnTicketWithoutResolveReminder_Expect_SetPending()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AddReminderTicketCommand(
                    new TicketFinalReminder(new TimeSpan(1, 1, 1), testData.Message, false)),
                ServiceProvider,
                new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);

            Assert.AreEqual(TicketStatus.Pending(TicketStatusType.New), evt.Status);
        }

        [Test]
        public async Task When_SetPendingOnAnsweredTicket_Expect_SetPendingAndCancelResolveLifecycles()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("Success")), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator());
            var statusEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);

            Assert.AreEqual(ticket.Id, statusEvt.AggregateId);
            Assert.AreEqual(TicketStatus.Pending(TicketStatusType.Answered), statusEvt.Status);

            AssertEventsNotPublished<TicketLifecyclePlannedEvent>(ticket);

            var lcCanceledEvent = AssertAndGetPublishedEvent<TicketLifecycleCanceledEvent>(ticket);
            Assert.IsTrue(lcCanceledEvent.LifeCycleType == TicketLifeCycleType.Resolving);
        }

        [Test]
        public async Task When_SetPendingOnAwaitingReplyTicket_Expect_SetPending()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("Success")), ServiceProvider, new SystemInitiator());
            await ticket.Execute(new AddIncomingMessageTicketCommand(testData.Message,
                testData.UserId, testData.Channel), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator());
            var statusEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);

            Assert.AreEqual(ticket.Id, statusEvt.AggregateId);
            Assert.AreEqual(TicketStatus.Pending(TicketStatusType.AwaitingReply), statusEvt.Status);

            AssertEventsNotPublished<TicketLifecyclePlannedEvent>(ticket);
        }


        [Test]
        public async Task When_SetPendingOnResolvedTicket_Expect_BrokeTicketMustHaveCorrectStatusRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("Success")), ServiceProvider, new SystemInitiator());
            await ticket.Execute(new ResolveTicketCommand(), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketMustHaveCorrectStatusRule>(async () =>
                await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task When_SetPendingOnForRejectedTicket_Expect_BrokeTicketMustHaveCorrectStatusRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message
            ), ServiceProvider, new SystemInitiator());
            await ticket.Execute(new RejectTicketCommand(new Message("test")), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketMustHaveCorrectStatusRule>(async () =>
                await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task When_RemovePendingOnAnsweredTicket_Expect_RemoveAndPlanningResolveLifecycle()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("Success")), ServiceProvider, new SystemInitiator());
            await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new TogglePendingTicketCommand(false), ServiceProvider, new SystemInitiator());

            var statusEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.IsTrue(statusEvt.Status.In(TicketStatusKind.Opened));

            var lcEvent = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            Assert.IsTrue(lcEvent.LifeCycleType == TicketLifeCycleType.Resolving);
        }

        #endregion

        #region Resolve

        /*
         * Cases:
         * Резолв нового - ошибка +
         * Резолв отвеченого - успех +
         * Резолв любого в пендинг - ошибка +
         * Резолв ожидающего ответ - ошибка
         * Резолв на ревью - ошибка
         * Резолв на апрове - ошибка
         * Резолв forReject - ошибка
         * Резолв резолвенного - успех - нет событий
         * Резолв с реминдером - отмена реминдеров
         * Резолв должен назначать на оператора если нет назначения - успех
         *
         */
        [Test]
        public async Task When_ResolveNewTicket_Expect_BreakTicketMustHaveCorrectStatusRule()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketMustHaveCorrectStatusRule>(async () =>
                await ticket.Execute(new ResolveTicketCommand(), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task
            When_ResolveOpenedAndAnsweredTicket_Expect_SetResolveStatusAndPlannedLifecycleCloseAndFeedback()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("Success")), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new ResolveTicketCommand(), ServiceProvider, new SystemInitiator());
            var statusEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);

            Assert.AreEqual(ticket.Id, statusEvt.AggregateId);
            Assert.AreEqual(TicketStatus.Opened(TicketStatusType.Resolved), statusEvt.Status);

            var lcPlanedEvents = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket);
            Assert.IsTrue(lcPlanedEvents.Count() == 2);
            Assert.IsTrue(lcPlanedEvents.Any(x => x.LifeCycleType == TicketLifeCycleType.Feedback));
            Assert.IsTrue(lcPlanedEvents.Any(x => x.LifeCycleType == TicketLifeCycleType.Closing));

            var lcCanceledEvent = AssertAndGetPublishedEvent<TicketLifecycleCanceledEvent>(ticket);
            Assert.IsTrue(lcCanceledEvent.LifeCycleType == TicketLifeCycleType.Resolving);
        }

        [Test]
        public async Task When_ResolvePendingAndAnsweredTicket_Expect_BrokeTicketMustHaveCorrectStatusRule()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("Success")), ServiceProvider, new SystemInitiator());
            await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketMustHaveCorrectStatusRule>(async () =>
                await ticket.Execute(new ResolveTicketCommand(), ServiceProvider, new SystemInitiator()));
        }

        #endregion

        #region Reject

        /*
         * Cases:
         * Реджект нового - успех +
         * Реджект любого пендинга - ошибка
         * Реджекс отвеченного - успех -> forReject
         * Реджекс решенного - ошибка
         * Апрров реджекта у нового - ошибка +
         * Отколнение реджекта у нового - ошибка +
         * Отмена реджекта у нового - ошибка +
         * Реджект при forReject - успех - нет событий
         * Реджект отвеченного без сообщения - ошибка
         * Реджект отвеченного с пустым сообщения - ошибка
         * Отклонение forReject - успех - установка пред состояния и если надо планирование таймеров
         * Отклонение forReject без сообщения - ошибка
         * Отклонение forReject с пустым сообщением - ошибка
         * Подтверждение forReject - успех - установка пред состояния и если надо планирование таймеров
         * Отмена forReject - успех - установка пред состояния и если надо планирование таймеров
         * Реджекс отвеченного должен назначать на оператора если нет назначения - успех
         * Реджекс новог должен назначать на оператора если нет назначенного - успех
         * Реджекс дожен отменять любой реминдер - успех
         * Реджекс отвеченного должен отменять любой реминдер - успех
         * Подтверждение forReject должен назначать на оператора если нет назначенного - успех
         * Отмена и отклонение должны возвращать к корректному статусу и пересоздавать таймеры - успех
         * Отмена или отколнение неизвестного аудита
         */
        [Test]
        public async Task When_RejectNewTicket_Expect_RejectAndClose()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RejectTicketCommand(), ServiceProvider, new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.AreEqual(TicketStatusKind.Closed, evt.Status.Kind);
            Assert.AreEqual(TicketStatusType.Rejected, evt.Status.Type);

            var evtClose = AssertAndGetPublishedEvent<TicketClosedEvent>(ticket);
            Assert.AreEqual(ticket.Id, evtClose.AggregateId);

            AssertEventsNotPublished<TicketLifecyclePlannedEvent>(ticket);
        }

        [Test]
        public async Task When_RejectWaitReplyTicket_Expect_GoToForRejectStatus()
        {
            ClearServices();
            var delay = new TimeSpan(0, 0, 0);
            ServiceProvider.Configurations.GetInactivityDelay(default).ReturnsForAnyArgs(Task.FromResult(delay));
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("Test")), ServiceProvider, new SystemInitiator());
            await ticket.Execute(new AddIncomingMessageTicketCommand(new Message("test"),
                testData.UserId, testData.Channel), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RejectTicketCommand(new Message("reject")), ServiceProvider,
                new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.AreEqual(TicketStatusKind.Opened, evt.Status.Kind);
            Assert.AreEqual(TicketStatusType.ForReject, evt.Status.Type);
            AssertEventsNotPublished<TicketClosedEvent>(ticket);
        }

        [Test]
        public async Task When_ApproveRejectOnNewTicket_Expect_BreakTicketCheckStatusRule()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketCheckStatusRule>(async () =>
                await ticket.Execute(new ApproveRejectTicketCommand(new TicketAuditId()), ServiceProvider,
                    new SystemInitiator()));
        }

        [Test]
        public async Task When_CancelRejectOnNewTicket_Expect_BreakTicketCheckStatusRule()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketCheckStatusRule>(async () =>
                await ticket.Execute(new CancelRejectTicketCommand(new TicketAuditId()), ServiceProvider,
                    new SystemInitiator()));
        }

        [Test]
        public async Task When_DenyRejectOnNewTicket_Expect_BreakTicketCheckStatusRule()
        {
            ClearServices();
            var ticket = await MakeTicket(new TestData());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketCheckStatusRule>(async () =>
                await ticket.Execute(new DenyRejectTicketCommand(new TicketAuditId(),
                    new Message("Deny")), ServiceProvider, new SystemInitiator()));
        }

        #endregion
    }
}
