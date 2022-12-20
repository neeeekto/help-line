using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Tests.Domain.SeedWork;
using NSubstitute;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Domain.Tickets
{
    [TestFixture]
    public class TicketFeedbacksTests : TicketTestsBase
    {
        /*
         * Cases
         * Получение фидбека +
         * Получение неизвестного фидбека +
         * Повторной фидбек (один и тотже) живой таймаут +
         * Повторной фидбек (один и тотже) мертвый таймаут +
         * Немедленная отправка фидбека - отмена таймера +
         * Немедленная отправка фидбека без активного таймера - ошибка +
         * Переоткрытие тикета при плохом фидбеке +
         */

        [Test]
        public async Task AllMethods_CloseTicket_BrokeTicketShouldNotBeClosedRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new RejectTicketCommand(), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            var feedback = new TicketFeedback(5, "test");
            AssertBrokenRule<TicketShouldNotBeClosedRule>(async () => await ticket.Execute(
                new SaveFeedbackTicketCommand(
                    new TicketFeedbackId(Guid.NewGuid()), feedback,
                    testData.UserId), ServiceProvider, new SystemInitiator()));
            AssertBrokenRule<TicketShouldNotBeClosedRule>(async () =>
                await ticket.Execute(new ImmediateSendFeedbackTicketCommand(), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task AllMethods_WithoutInitiator_BrokeInitiatorMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<InitiatorMustExistRule>(async () =>
                await ticket.Execute(new ImmediateSendFeedbackTicketCommand(), ServiceProvider, null!));
        }

        [Test]
        public async Task When_AddFeedback_Expect_Adding()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var feedbackEvt = await ResolveAndSendFeedback(ticket);
            ticket.ClearUncommittedEvents();

            var feedback = new TicketFeedback(5, "test");
            await ticket.Execute(new SaveFeedbackTicketCommand(feedbackEvt.FeedbackId,
                feedback,
                testData.UserId), ServiceProvider, new SystemInitiator());

            var evt = AssertAndGetPublishedEvent<TicketFeedbackAddedEvent>(ticket);
            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(feedbackEvt.FeedbackId, evt.FeedbackId);
            Assert.AreEqual(feedback, evt.Feedback);

            AssertEventsNotPublished<TicketStatusChangedEvent>(ticket);
            AssertEventsNotPublished<TicketClosedEvent>(ticket);
        }

        [Test]
        public async Task When_AddFeedback_Expect_CallDispatcher()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var feedbackEvt = await ResolveAndSendFeedback(ticket);
            ticket.ClearUncommittedEvents();

            var feedback = new TicketFeedback(5, "test");
            await ticket.Execute(new SaveFeedbackTicketCommand(feedbackEvt.FeedbackId,
                feedback,
                testData.UserId), ServiceProvider, new SystemInitiator());

            Channel? channel;
            ServiceProvider.FeedbackDispatcher.Received(1).Enqueue(Arg.Is(ticket.Id), Arg.Is(feedbackEvt.FeedbackId),
                Arg.Is<IEnumerable<UserChannel>>(channels =>
                    channels.Any(x => x.UserId == testData.UserId && x.Channel == testData.Channel)),
                Arg.Is(testData.ProjectId));
        }

        [Test]
        public async Task When_AddUnknownFeedback_Expect_BrokeTicketFeedbackMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ResolveAndSendFeedback(ticket);
            ticket.ClearUncommittedEvents();

            var feedback = new TicketFeedback(5, "test");
            AssertBrokenRule<TicketFeedbackMustExistRule>(async () => await ticket.Execute(
                new SaveFeedbackTicketCommand(
                    new TicketFeedbackId(Guid.NewGuid()), feedback,
                    testData.UserId), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task When_AddSameFeedbackBeforeTimeout_Expect_Adding()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var feedbackEvt = await ResolveAndSendFeedback(ticket);

            ServiceProvider.Configurations.GetFeedbackCompleteDelay(default)
                .ReturnsForAnyArgs(Task.FromResult(new TimeSpan(1, 0, 0)));
            var feedback = new TicketFeedback(5, "test");
            await ticket.Execute(new SaveFeedbackTicketCommand(feedbackEvt.FeedbackId,
                feedback,
                testData.UserId), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();


            var newFeedback = new TicketFeedback(3, "tes2");
            await ticket.Execute(new SaveFeedbackTicketCommand(feedbackEvt.FeedbackId,
                newFeedback,
                testData.UserId), ServiceProvider, new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketFeedbackAddedEvent>(ticket);
            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(feedbackEvt.FeedbackId, evt.FeedbackId);
            Assert.AreEqual(newFeedback, evt.Feedback);
        }

        [Test]
        public async Task
            When_AddSameFeedbackAfterTimeout_Expect_BrokeTicketFeedbackMustBeAvailableForModificationRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var feedbackEvt = await ResolveAndSendFeedback(ticket);
            ServiceProvider.Configurations.GetFeedbackCompleteDelay(default)
                .ReturnsForAnyArgs(Task.FromResult(new TimeSpan(1)));
            var feedback = new TicketFeedback(5, "test");
            await ticket.Execute(new SaveFeedbackTicketCommand(feedbackEvt.FeedbackId,
                feedback,
                testData.UserId), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();


            var newFeedback = new TicketFeedback(3, "tes2");
            AssertBrokenRule<TicketFeedbackMustBeAvailableForModificationRule>(async () =>
                await ticket.Execute(new SaveFeedbackTicketCommand(
                    feedbackEvt.FeedbackId, newFeedback,
                    testData.UserId), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task When_ReopenTicketByFeedback_Expect_Reopen()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var feedbackEvt = await ResolveAndSendFeedback(ticket);

            ServiceProvider.ReopenChecker.CheckBy(default!, default!).ReturnsForAnyArgs(Task.FromResult(true));
            ticket.ClearUncommittedEvents();

            var feedback = new TicketFeedback(5, "test");
            await ticket.Execute(new SaveFeedbackTicketCommand(feedbackEvt.FeedbackId,
                feedback,
                testData.UserId), ServiceProvider, new SystemInitiator());

            var statusEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.AreEqual(TicketStatus.Opened(TicketStatusType.AwaitingReply), statusEvt.Status);

            var lcCancelEvt = AssertAndGetPublishedEvent<TicketLifecycleCanceledEvent>(ticket);
            Assert.AreEqual(TicketLifeCycleType.Closing, lcCancelEvt.LifeCycleType);

            AssertEventsNotPublished<TicketLifecyclePlannedEvent>(ticket);
        }

        [Test]
        public async Task When_ReopenTicketByFeedbackWithAssign_Expect_ReopenAndUnassign()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(testData.OperatorId);
            var feedbackEvt = await ResolveAndSendFeedback(ticket, initiator);
            ServiceProvider.ReopenChecker.CheckBy(default!, default!).ReturnsForAnyArgs(Task.FromResult(true));
            ticket.ClearUncommittedEvents();

            var feedback = new TicketFeedback(5, "test");
            await ticket.Execute(new SaveFeedbackTicketCommand(feedbackEvt.FeedbackId,
                feedback,
                testData.UserId), ServiceProvider, new SystemInitiator());

            var assignEvt = AssertAndGetPublishedEvent<TicketAssignChangedEvent>(ticket);
            Assert.IsNull(assignEvt.Assignee);
        }

        [Test]
        public async Task When_ReopenTicketByFeedbackWithHardAssign_Expect_ReopenWithoutUnassign()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var initiator = new OperatorInitiator(testData.OperatorId);
            var feedbackEvt = await ResolveAndSendFeedback(ticket, initiator);
            ServiceProvider.ReopenChecker.CheckBy(default!, default!).ReturnsForAnyArgs(Task.FromResult(true));
            await ticket.Execute(new ChangeHardAssigmentTicketCommand(true), ServiceProvider, initiator);
            ticket.ClearUncommittedEvents();

            var feedback = new TicketFeedback(5, "test");
            await ticket.Execute(new SaveFeedbackTicketCommand(feedbackEvt.FeedbackId,
                feedback,
                testData.UserId), ServiceProvider, new SystemInitiator());

            AssertEventsNotPublished<TicketAssignChangedEvent>(ticket);
        }

        [Test]
        public async Task When_ImmediateSendForPlanned_Expect_SendAndCancelTimer()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var feedbackTimer = await PrepareFeedback(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new ImmediateSendFeedbackTicketCommand(), ServiceProvider, new SystemInitiator());

            var assignEvt = AssertAndGetPublishedEvent<TicketFeedbackSentEvent>(ticket);
            Assert.AreEqual(ticket.Id, assignEvt.AggregateId);
            Assert.IsNotNull(assignEvt.FeedbackId);

            var cancelLcEvt = AssertAndGetPublishedEvent<TicketLifecycleCanceledEvent>(ticket);
            Assert.AreEqual(feedbackTimer.ScheduleId, cancelLcEvt.ScheduleId);
        }

        [Test]
        public async Task When_ImmediateSendForNotPlanned_Expect_BrokeTicketShouldBeScheduledForRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketShouldBeScheduledForRule>(async () =>
                await ticket.Execute(new ImmediateSendFeedbackTicketCommand(), ServiceProvider, new SystemInitiator()));
        }

        [Test]
        public async Task When_SendFeedbackIfUserHasNoActiveChannels_Expect_NoAction()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var feedbackTimer = await PrepareFeedback(ticket);
            await ticket.Execute(new ToggleUserChannelTicketCommand(testData.UserId, false), ServiceProvider,
                testData.Initiator);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new ImmediateSendFeedbackTicketCommand(), ServiceProvider, new SystemInitiator());
            Assert.Throws<EventsNotPublishedException<TicketFeedbackSentEvent>>(() =>
                AssertAndGetPublishedEvents<TicketFeedbackSentEvent>(ticket));
        }

        [Test]
        public async Task When_TicketCreatedByOperatorOrSystem_Expect_DoNotSendFeedback()
        {
            ClearServices();
            var testData = new TestData();
            testData.Initiator = new SystemInitiator();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new TogglePendingTicketCommand(false), ServiceProvider, testData.Initiator);
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("Resolve")), ServiceProvider, new SystemInitiator());
            ServiceProvider.Configurations.GetInactivityDelay(default)
                .ReturnsForAnyArgs(Task.FromResult(new TimeSpan(0, 0, 0)));
            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());
            var lcTimers = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket);
            var feedbackTimer = lcTimers.FirstOrDefault(x => x.LifeCycleType == TicketLifeCycleType.Feedback);
            Assert.IsNull(feedbackTimer);
        }

        private async Task<TicketFeedbackSentEvent> ResolveAndSendFeedback(Ticket ticket, Initiator? initiator = null)
        {
            var feedbackTimer = await PrepareFeedback(ticket, initiator);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(
                feedbackTimer!.ScheduleId), ServiceProvider, new SystemInitiator());
            return AssertAndGetPublishedEvent<TicketFeedbackSentEvent>(ticket);
        }

        private async Task<TicketLifecyclePlannedEvent> PrepareFeedback(Ticket ticket, Initiator? initiator = null)
        {
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("Resolve")), ServiceProvider, initiator ?? new SystemInitiator());
            ServiceProvider.Configurations.GetInactivityDelay(default)
                .ReturnsForAnyArgs(Task.FromResult(new TimeSpan(0, 0, 0)));
            var resolveTimer = AssertAndGetPublishedEvent<TicketLifecyclePlannedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RunTicketScheduleCommand(
                resolveTimer.ScheduleId), ServiceProvider, new SystemInitiator());
            var lcTimers = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket);
            var feedbackTimer = lcTimers.FirstOrDefault(x => x.LifeCycleType == TicketLifeCycleType.Feedback);
            return feedbackTimer!;
        }
    }
}
