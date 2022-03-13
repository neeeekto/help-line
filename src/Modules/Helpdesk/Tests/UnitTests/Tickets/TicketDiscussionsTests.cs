using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using NSubstitute;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Domain.UnitTests.Tickets
{
    [TestFixture]
    public class TicketDiscussionsTests : TicketTestsBase
    {
        /*
         * Cases
         * Входящее сообщение от известного +
         * Входящее сообщение от неизвестного пользователя - ??? +
         * Входящее сообщение без сообщения - ошибка +
         * Входящее сообщение с пустым сообщением - ошибка +
         * Входящее сообщение от известного пользотеля но с другого канала - ??? +
         * Входящее сообщение меняет статус на ожидающий ответа для тронутого +
         * Входящее сообщение не меняет статус для нетронутого +
         * Входящее сообщение снимает назначение если нет Hard +
         * Входящее сообщение не снимает назначение если есть Hard +
         * Входящее сообщение при forReject - отмена forReject +
         * Входящее сообщение отменяет все ремайндеры +
         *
         * Исходящее на новом - перевод в answered и планирование таймеров +
         * Исходящее без каналов - ошибка TicketOutgoingMessageChannelsCannotBeEmptyRule +
         * Исходящее вызывает метод Enqueue диспатчера сообщений +
         * Исходящее отменяет ремайндеры +
         * Исходящее назначает на оператора если оператор +
         * Исходящее не назначает на систему если система +
         *
         * AddMessageStatus на существ сообщение - успех +
         * AddMessageStatus на не существ  сообщение - ошибка +
         * AddMessageStatus на сущ сообщение с неизвестным userId - ошибка +
         *
         * Повторная отправка на недоставленном - успех +
         * Повторная отправка на доставленом - успех +
         * Повторная отправка на отправляемом - ошибка (уже в очереди же) +
         * Повторная отправка несуществующего сообщения - ошибка +
         * Повторная отправка неизвестного UserID - ошибка +
         */

        [Test]
        public async Task AllMethods_WithoutInitiator_BreakWithInitiatorMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<InitiatorMustExistRule>(async () =>
                await ticket.Execute(new AddOutgoingMessageTicketCommand(new Message("test")), ServiceProvider, null!)
            );

            AssertBrokenRule<InitiatorMustExistRule>(async () =>
                await ticket.Execute(
                    new RetryOutgoingMessageTicketCommand(new TicketOutgoingMessageId(Guid.NewGuid()),
                        testData.UserId), ServiceProvider, null!)
            );
        }

        [Test]
        public async Task AllMethods_CloseTicket_BreakTicketShouldNotBeClosedRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new RejectTicketCommand(), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketShouldNotBeClosedRule>(async () =>
                await ticket.Execute(new AddOutgoingMessageTicketCommand(testData.Message), ServiceProvider,
                    testData.Initiator)
            );
            AssertBrokenRule<TicketShouldNotBeClosedRule>(async () =>
                await ticket.Execute(
                    new AddIncomingMessageTicketCommand(testData.Message, testData.UserId, testData.Channel),
                    ServiceProvider, new SystemInitiator())
            );

            AssertBrokenRule<TicketShouldNotBeClosedRule>(async () =>
                await ticket.Execute(new RetryOutgoingMessageTicketCommand(new TicketOutgoingMessageId(Guid.NewGuid()),
                    testData.UserId), ServiceProvider, testData.Initiator)
            );
        }

        #region IncomingMessage

        [Test]
        public async Task When_IncomingMessageWithEmptyMessage_Expect_BrokeMessageCannotBeEmptyRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<MessageCannotBeEmptyRule>(async () => await ticket.Execute(
                new AddIncomingMessageTicketCommand(
                    null!,
                    testData.UserId, testData.Channel), ServiceProvider, new SystemInitiator()));
            AssertBrokenRule<MessageCannotBeEmptyRule>(async () => await ticket.Execute(
                new AddIncomingMessageTicketCommand(
                    new Message(""),
                    testData.UserId, testData.Channel), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task When_IncomingMessageByExistUser_Expect_Success()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddIncomingMessageTicketCommand(
                testData.Message,
                testData.UserId, testData.Channel), ServiceProvider, new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketIncomingMessageAddedEvent>(ticket);

            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(testData.Message, evt.Message);
            Assert.AreEqual(testData.Channel, evt.Channel);
            Assert.AreEqual(testData.Message, evt.Message);
        }

        [Test]
        public async Task When_IncomingMessageByNotExistUserId_Expect_BrokeUserIdMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<UserIdMustExistRule>(async () =>
                await ticket.Execute(new AddIncomingMessageTicketCommand(
                        testData.Message,
                        new UserId("23432"), testData.Channel),
                    ServiceProvider,
                    new SystemInitiator()
                )
            );
        }

        [Test]
        public async Task When_IncomingMessageByNotExistChanel_Expect_BrokeUserIdMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<UserIdMustExistRule>(async () =>
                await ticket.Execute(new AddIncomingMessageTicketCommand(
                        testData.Message,
                        testData.UserId, new Channel("chat")),
                    ServiceProvider,
                    new SystemInitiator()
                )
            );
        }

        [Test]
        public async Task When_IncomingMessageForNewTicket_Expect_NoChangeStatus()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddIncomingMessageTicketCommand(
                    testData.Message,
                    testData.UserId, testData.Channel),
                ServiceProvider,
                new SystemInitiator()
            );
            AssertEventsNotPublished<TicketStatusChangedEvent>(ticket);
        }

        [Test]
        public async Task When_IncomingMessageForAwaitReply_Expect_NoChangeStatus()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                    testData.Message),
                ServiceProvider,
                new SystemInitiator()
            );
            await ticket.Execute(new AddIncomingMessageTicketCommand(
                    testData.Message,
                    testData.UserId, testData.Channel),
                ServiceProvider,
                new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddIncomingMessageTicketCommand(
                    testData.Message,
                    testData.UserId, testData.Channel),
                ServiceProvider,
                new SystemInitiator()
            );
            AssertEventsNotPublished<TicketStatusChangedEvent>(ticket);
        }

        [Test]
        public async Task When_IncomingMessageForAnswered_Expect_ChangeStatusToAwaitReply()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                    testData.Message),
                ServiceProvider,
                new SystemInitiator()
            );
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddIncomingMessageTicketCommand(
                    testData.Message,
                    testData.UserId, testData.Channel),
                ServiceProvider,
                new SystemInitiator()
            );
            var evt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.AreEqual(TicketStatus.Opened(TicketStatusType.AwaitingReply), evt.Status);
        }

        [Test]
        public async Task When_IncomingMessageForNonHardAssigment_Expect_Unassign()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                    testData.Message),
                ServiceProvider,
                new OperatorInitiator(new OperatorId(Guid.NewGuid()))
            );
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddIncomingMessageTicketCommand(
                    testData.Message,
                    testData.UserId, testData.Channel),
                ServiceProvider,
                new SystemInitiator()
            );
            var evt = AssertAndGetPublishedEvent<TicketAssignChangedEvent>(ticket);
            Assert.IsNull(evt.Assignee);
        }

        [Test]
        public async Task When_IncomingMessageForHardAssigment_Expect_DoNotChangeAssigment()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                    testData.Message),
                ServiceProvider,
                new OperatorInitiator(new OperatorId(Guid.NewGuid()))
            );

            await ticket.Execute(new ChangeHardAssigmentTicketCommand(true), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddIncomingMessageTicketCommand(
                    testData.Message,
                    testData.UserId, testData.Channel),
                ServiceProvider,
                new SystemInitiator()
            );
            AssertEventsNotPublished<TicketAssignChangedEvent>(ticket);
        }

        [Test]
        public async Task When_IncomingMessageIfForRejectStatus_Expect_ChangeForReject()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, new OperatorInitiator(new OperatorId(Guid.NewGuid())));
            await ticket.Execute(new RejectTicketCommand(new Message("Test")), ServiceProvider, new SystemInitiator());
            var rejectEvt = AssertAndGetPublishedEvent<TicketApprovalStatusAddedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddIncomingMessageTicketCommand(
                    testData.Message,
                    testData.UserId, testData.Channel),
                ServiceProvider,
                new SystemInitiator()
            );
            var cancelEvent = AssertAndGetPublishedEvent<TicketApprovalStatusCanceledEvent>(ticket);
            Assert.AreEqual(rejectEvt.AuditId, cancelEvent.AuditId);

            var statusEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.AreEqual(TicketStatus.Opened(TicketStatusType.AwaitingReply), statusEvt.Status);
        }


        [Test]
        public async Task When_IncomingMessage_Expect_CancelReminders()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var reminder = new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("reminder"), true);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddReminderTicketCommand(reminder), ServiceProvider, initiator);

            AssertEventsPublished<TicketReminderScheduledEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddIncomingMessageTicketCommand(
                    testData.Message,
                    testData.UserId, testData.Channel),
                ServiceProvider,
                new SystemInitiator()
            );
            AssertEventsPublished<TicketReminderCanceledEvent>(ticket);
        }

        [Test]
        public async Task When_IncomingMessageAfterFirstReminder_Expect_CancelSecondReminder()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var secondReminder = new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("reminder"), true);
            var firstReminder =
                new TicketSequentialReminder(new TimeSpan(1, 0, 0), new Message("reminder"), secondReminder);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddReminderTicketCommand(firstReminder), ServiceProvider, initiator);
            var reminderEvt = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(reminderEvt.ScheduleId),
                ServiceProvider,
                new SystemInitiator()
            );
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddIncomingMessageTicketCommand(
                    testData.Message,
                    testData.UserId, testData.Channel),
                ServiceProvider,
                new SystemInitiator()
            );
            AssertEventsPublished<TicketReminderCanceledEvent>(ticket);
        }

        [Test]
        public async Task When_IncomingMessage_Expect_CallTrySubscribeMethodOnUserService()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddIncomingMessageTicketCommand(
                    testData.Message,
                    testData.UserId, testData.Channel),
                ServiceProvider,
                new SystemInitiator()
            );
            ServiceProvider.UnsubscribeManager.Received(1)
                .TryRemove(Arg.Is(testData.UserId), Arg.Is(testData.ProjectId));
        }

        #endregion

        #region OutgoingMessage

        [Test]
        public async Task When_OutgoingMessageOnNewTicket_Expect_ChangeStatusToAnsweredAndPlanningLifecycle()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                    testData.Message),
                ServiceProvider,
                initiator
            );

            var messageEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.AreEqual(ticket.Id, messageEvt.AggregateId);
            Assert.AreEqual(initiator, messageEvt.Initiator);
            Assert.AreEqual(testData.Message, messageEvt.Message);
            Assert.AreEqual(ticket.State.User.Channels, messageEvt.Recipients);
            Assert.IsNotNull(messageEvt.MessageId);

            var statusEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.AreEqual(TicketStatus.Opened(TicketStatusType.Answered), statusEvt.Status);

            var lcEvt = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            Assert.AreEqual(TicketLifeCycleType.Resolving, lcEvt.LifeCycleType);
        }

        [Test]
        public async Task When_OutgoingMessageOnAnswereTicket_Expect_NoChangeStatusAndNotPlanningLifecycle()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                    testData.Message),
                ServiceProvider,
                initiator
            );
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                    testData.Message),
                ServiceProvider,
                initiator
            );

            AssertEventsNotPublished<TicketStatusChangedEvent>(ticket);
            AssertEventsNotPublished<TicketLifecyclePlannedEvent>(ticket);
            AssertEventsNotPublished<TicketLifecycleCanceledEvent>(ticket);
        }

        [Test]
        public async Task
            When_OutgoingMessageOnNewPendingTicket_Expect_ChangeStatusToAnsweredAndDoNotPlanningLifecycle()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new TogglePendingTicketCommand(true), ServiceProvider, initiator);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                    testData.Message),
                ServiceProvider,
                initiator
            );

            AssertEventsPublished<TicketOutgoingMessageAddedEvent>(ticket);
            AssertEventsPublished<TicketStatusChangedEvent>(ticket);
            AssertEventsNotPublished<TicketLifecyclePlannedEvent>(ticket);
        }

        [Test]
        public async Task When_OutgoingMessageOnTicketWithoutChannels_Expect_BrokeUserChannelsCannotBeEmptyRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new ToggleUserChannelTicketCommand(testData.UserId, false), ServiceProvider,
                new SystemInitiator());
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<UserChannelsCannotBeEmptyRule>(async () =>
                await ticket.Execute(new AddOutgoingMessageTicketCommand(
                        testData.Message),
                    ServiceProvider,
                    new SystemInitiator())
            );
        }

        [Test]
        public async Task When_OutgoingMessageWithEmptyMessage_Expect_BrokeMessageCannotBeEmptyRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<MessageCannotBeEmptyRule>(async () => await ticket.Execute(
                new AddOutgoingMessageTicketCommand(
                    null!), ServiceProvider, new SystemInitiator()));
            AssertBrokenRule<MessageCannotBeEmptyRule>(async () => await ticket.Execute(
                new AddOutgoingMessageTicketCommand(
                    new Message("")), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task When_OutgoingMessage_Expect_CallEnqueueMethodOnMessageDispatcher()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Channel? channel;
            ServiceProvider.MessageDispatcher.Received(1).Enqueue(Arg.Is(ticket.Id), Arg.Is(evt.MessageId),
                Arg.Is(testData.Message),
                Arg.Is<IEnumerable<UserChannel>>(channels =>
                    channels.Any(x => x.UserId == testData.UserId && x.Channel == testData.Channel)),
                Arg.Is(testData.ProjectId));
        }

        [Test]
        public async Task
            When_OutgoingMessageOnNewTicketByOperator_Expect_AssignToOperator()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator);

            var evt = AssertAndGetPublishedEvent<TicketAssignChangedEvent>(ticket);
            Assert.AreEqual(initiator.OperatorId, evt.Assignee);
        }

        [Test]
        public async Task
            When_OutgoingMessageOnTicketWithHardAssigmentByOperator_Expect_NotAssign()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new SystemInitiator();
            await ticket.Execute(new AssignTicketCommand(new OperatorInitiator(new OperatorId(Guid.NewGuid()))),
                ServiceProvider, new SystemInitiator());
            await ticket.Execute(new ChangeHardAssigmentTicketCommand(true), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                    testData.Message),
                ServiceProvider,
                initiator
            );

            AssertEventsNotPublished<TicketAssignChangedEvent>(ticket);
        }

        [Test]
        public async Task
            When_OutgoingMessageOnTicketWithAssigmentByOperator_Expect_ReassignToInitiator()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator1 = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            var initiator2 = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator1);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator2);


            var evt = AssertAndGetPublishedEvent<TicketAssignChangedEvent>(ticket);
            Assert.AreEqual(initiator2.OperatorId, evt.Assignee);
        }

        [Test]
        public async Task
            When_OutgoingMessageOnNewTicketBySystem_Expect_NotAssign()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new SystemInitiator();
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator);

            AssertEventsNotPublished<TicketAssignChangedEvent>(ticket);
        }

        [Test]
        public async Task When_OutgoingMessage_Expect_CancelAllReminders()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var reminder = new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("reminder"), true);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddReminderTicketCommand(reminder), ServiceProvider, initiator);
            var reminderEvt = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator);
            var cancelEvt = AssertAndGetPublishedEvent<TicketReminderCanceledEvent>(ticket);
            Assert.AreEqual(reminderEvt.Reminder.Id, cancelEvt.ReminderId);
        }

        [Test]
        public async Task When_OutgoingMessageAfterFirstReminder_Expect_CancelSecondReminders()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var secondReminder = new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("reminder"), true);
            var firstReminder =
                new TicketSequentialReminder(new TimeSpan(1, 0, 0), new Message("reminder"), secondReminder);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddReminderTicketCommand(firstReminder), ServiceProvider, initiator);
            var reminderEvt = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(reminderEvt.ScheduleId), ServiceProvider,
                new SystemInitiator());
            var reminderEvt2 = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator);
            var cancelEvt = AssertAndGetPublishedEvent<TicketReminderCanceledEvent>(ticket);
            Assert.AreEqual(reminderEvt2.Reminder.Id, cancelEvt.ReminderId);
        }

        #endregion

        #region AddStatus

        [Test]
        public async Task When_AddStatusForExistMessageAndUser_Expect_Adding()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator);
            var messageEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(
                new AddMessageStatusTicketCommand(messageEvt.MessageId, testData.UserId, MessageStatus.Delivered),
                ServiceProvider,
                new SystemInitiator()
            );
            var evt = AssertAndGetPublishedEvent<TicketMessageStatusChangedEvent>(ticket);

            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(MessageStatus.Delivered, evt.Status);
            Assert.AreEqual(testData.UserId, evt.UserId);
        }

        [Test]
        public async Task When_AddStatusForNotExistMessage_Expect_BrokeTicketOutgoingMessageMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketOutgoingMessageMustExistRule>(() =>
                ticket.Execute(new AddMessageStatusTicketCommand(new TicketOutgoingMessageId(Guid.NewGuid()),
                    testData.UserId,
                    MessageStatus.Delivered), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task When_AddStatusForExistUserId_Expect_BrokeUserIdMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator);
            var messageEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<UserIdMustExistRule>(() =>
                ticket.Execute(new AddMessageStatusTicketCommand(messageEvt.MessageId, new UserId("111"),
                    MessageStatus.Delivered), ServiceProvider, new SystemInitiator())
            );
        }

        #endregion

        #region Resend

        [Test]
        public async Task When_RetryExistNotDeliveredMessage_Expect_ChangeMessageStatusToSendingAndCallEnqueue()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator);
            var messageEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            ticket.Execute(new AddMessageStatusTicketCommand(messageEvt.MessageId, testData.UserId,
                MessageStatus.NotDelivered), ServiceProvider, new SystemInitiator());
            ServiceProvider.MessageDispatcher.ClearReceivedCalls();
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RetryOutgoingMessageTicketCommand(messageEvt.MessageId, testData.UserId),
                ServiceProvider, initiator);
            var evt = AssertAndGetPublishedEvent<TicketMessageStatusChangedEvent>(ticket);

            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(MessageStatus.Sending, evt.Status);
            Assert.AreEqual(testData.UserId, evt.UserId);

            Channel channel;
            ServiceProvider.MessageDispatcher.Received(1).Enqueue(Arg.Is(ticket.Id), Arg.Is(evt.MessageId),
                Arg.Is(testData.Message),
                Arg.Is<IEnumerable<UserChannel>>(channels =>
                    channels.Any(x => x.UserId == testData.UserId && x.Channel == testData.Channel)),
                Arg.Is(testData.ProjectId));
        }

        [Test]
        public async Task When_RetryExistDeliveredMessage_Expect_ChangeMessageStatusToSendingAndCallEnqueue()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator);
            var messageEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            ticket.Execute(new AddMessageStatusTicketCommand(messageEvt.MessageId, testData.UserId,
                MessageStatus.Delivered), ServiceProvider, new SystemInitiator());
            ServiceProvider.MessageDispatcher.ClearReceivedCalls();
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RetryOutgoingMessageTicketCommand(messageEvt.MessageId, testData.UserId),
                ServiceProvider, initiator);
            var evt = AssertAndGetPublishedEvent<TicketMessageStatusChangedEvent>(ticket);

            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(MessageStatus.Sending, evt.Status);
            Assert.AreEqual(testData.UserId, evt.UserId);

            Channel channel;
            ServiceProvider.MessageDispatcher.Received(1).Enqueue(Arg.Is(ticket.Id), Arg.Is(evt.MessageId),
                Arg.Is(testData.Message),
                Arg.Is<IEnumerable<UserChannel>>(channels =>
                    channels.Any(x => x.UserId == testData.UserId && x.Channel == testData.Channel)),
                Arg.Is(testData.ProjectId));
        }

        [Test]
        public async Task When_RetrySendingMessage_Expect_BrokeTicketOutgoingMessageMustNotSendingRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator);
            var messageEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketOutgoingMessageMustNotSendingRule>(async () =>
                await ticket.Execute(new RetryOutgoingMessageTicketCommand(messageEvt.MessageId, testData.UserId),
                    ServiceProvider, initiator));
        }

        [Test]
        public async Task When_RetryNotExistMessage_Expect_BrokeTicketOutgoingMessageMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketOutgoingMessageMustExistRule>(async () =>
                await ticket.Execute(new RetryOutgoingMessageTicketCommand(
                    new TicketOutgoingMessageId(Guid.NewGuid()), testData.UserId), ServiceProvider, initiator));
        }

        [Test]
        public async Task When_RetryMessageForUnknowUserId_Expect_BrokeUserIdMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(new OperatorId(Guid.NewGuid()));
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                testData.Message), ServiceProvider, initiator);
            var messageEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<UserIdMustExistRule>(async () => await ticket.Execute(
                new RetryOutgoingMessageTicketCommand(
                    messageEvt.MessageId, new UserId("1111")), ServiceProvider, initiator)
            );
        }

        #endregion
    }
}
