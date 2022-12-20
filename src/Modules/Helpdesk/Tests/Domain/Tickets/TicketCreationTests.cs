using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Tests.Domain.SeedWork;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Domain.Tickets
{
    [TestFixture]
    public class TicketCreationTests : TicketTestsBase
    {

        [Test]
        public async Task When_Creation_Expect_IdFactoryCalling()
        {
            ClearServices();

            var testData = new TestData();
            await MakeTicket(testData);

            await ServiceProvider.IdFactory.Received(1)
                .GetNext(Arg.Any<ProjectId>());
        }

        [Test]
        public async Task When_CreateByUserWithoutAutoreply_Expect_AutoreplyWillNotSend()
        {
            ClearServices();

            var testData = new TestData();
            var ticket = await MakeTicket(testData);

            var evt = AssertAndGetPublishedEvent<TicketCreatedEvent>(ticket);
            Assert.AreEqual(TicketId, evt.AggregateId);
            Assert.AreEqual(testData.LanguageCode, evt.Language);
            Assert.AreEqual(testData.Message, evt.Message);
            Assert.AreEqual(TicketPriority.Normal, evt.Priority);
            Assert.AreEqual(TicketStatusKind.Opened, evt.Status.Kind);
            Assert.AreEqual(TicketStatusType.New, evt.Status.Type);
            Assert.IsTrue(evt.Tags.Contains(testData.Tag));
            Assert.AreEqual(evt.Tags.Count(), testData.Tags.Count());
            Assert.IsTrue(evt.UserChannels.Any(x => x.UserId == testData.UserId));
            Assert.IsTrue(evt.UserMeta == testData.UserMeta);
        }

        [Test]
        public async Task When_CreateByUserWithAutoreply_Expect_SendAutoreply()
        {
            ClearServices();
            ServiceProvider.Autoreplies.ClearSubstitute();
            ServiceProvider.Autoreplies.Get(default).ReturnsForAnyArgs(
                Task.FromResult(new Autoreply(
                    "test",
                    new Message("autoreply"),
                    new[] {new Tag("autoreply")},
                    false)));

            var ticket = await MakeTicket(new TestData());

            var autoreplyEvent = AssertAndGetPublishedEvent<TicketAutoreplySendedEvent>(ticket);
            Assert.AreEqual(autoreplyEvent.AggregateId, TicketId);
            Assert.AreEqual(autoreplyEvent.Message, new Message("autoreply"));
            Assert.IsNull(autoreplyEvent.Reminder);
            Assert.IsInstanceOf<SystemInitiator>(autoreplyEvent.Initiator);
            Assert.IsFalse(autoreplyEvent.Resolve);
            Assert.IsTrue(autoreplyEvent.Tags.Count() == 1);
            Assert.IsTrue(autoreplyEvent.Tags.Contains(new Tag("autoreply")));

            var outgoingEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.AreEqual(outgoingEvt.AggregateId, TicketId);
            Assert.AreEqual(outgoingEvt.Message, new Message("autoreply"));
            Assert.IsNotNull(outgoingEvt.MessageId);
        }

        [Test]
        public async Task When_CreateByUserWithAutoreplyAndReminder_Expect_SendAutoreplyAndAddReminder()
        {
            ClearServices();
            ServiceProvider.Autoreplies.ClearSubstitute();
            ServiceProvider.Autoreplies.Get(default).ReturnsForAnyArgs(
                Task.FromResult(new Autoreply(
                    "test",
                    new Message("autoreply"),
                    new[] {new Tag("autoreply")},
                    new TicketFinalReminder(new TimeSpan(1, 0, 0), new Message("Reminder"), false))));

            var ticket = await MakeTicket(new TestData());

            var autoreplyEvent = AssertAndGetPublishedEvent<TicketAutoreplySendedEvent>(ticket);
            Assert.IsNotNull(autoreplyEvent.Reminder);
            Assert.AreEqual(autoreplyEvent.Reminder!.Delay, new TimeSpan(1, 0, 0));
            Assert.AreEqual(autoreplyEvent.Reminder.Message, new Message("Reminder"));

            var reminderEvt = AssertAndGetPublishedEvent<TicketReminderScheduledEvent>(ticket);
            Assert.IsNotNull(reminderEvt.ScheduleId);
            Assert.AreEqual(reminderEvt.Reminder.Message, new Message("Reminder"));
            Assert.AreEqual(reminderEvt.Reminder.Delay, new TimeSpan(1, 0, 0));
            Assert.IsInstanceOf<SystemInitiator>(reminderEvt.Initiator);
        }

        [Test]
        public async Task When_CreateByUserWithAutoreplyWithResolve_Expect_SendAutoreplyAndResolve()
        {
            ClearServices();

            ServiceProvider.Autoreplies.ClearSubstitute();
            ServiceProvider.Autoreplies.Get(default).ReturnsForAnyArgs(
                Task.FromResult(new Autoreply(
                    "test",
                    new Message("autoreply"),
                    new[] {new Tag("autoreply")},
                    true)));

            var ticket = await MakeTicket(new TestData());

            var autoreplyEvent = AssertAndGetPublishedEvent<TicketAutoreplySendedEvent>(ticket);
            Assert.IsNull(autoreplyEvent.Reminder);
            Assert.IsTrue(autoreplyEvent.Resolve);

            var statusChangedEvt = AssertAndGetPublishedEvent<TicketStatusChangedEvent>(ticket);
            Assert.AreEqual(TicketStatusKind.Opened, statusChangedEvt.Status.Kind);
            Assert.AreEqual(TicketStatusType.Resolved, statusChangedEvt.Status.Type);

            var timerEvts = AssertAndGetPublishedEvents<TicketLifecyclePlannedEvent>(ticket);
            Assert.IsTrue(timerEvts.All(x => x.LifeCycleType != TicketLifeCycleType.Resolving));
            Assert.IsTrue(timerEvts.Any(x => x.LifeCycleType == TicketLifeCycleType.Closing));
            Assert.IsTrue(timerEvts.Any(x => x.LifeCycleType == TicketLifeCycleType.Feedback));
        }

        [Test]
        public async Task When_CreateByUserWithEmptyMessage_Expect_BrokeTicketMessageCannotBeEmptyRule()
        {
            ClearServices();

            var testData = new TestData();
            testData.Message = null;

            AssertBrokenRule<MessageCannotBeEmptyRule>(async () => { await MakeTicket(testData); });
        }

        [Test]
        public async Task When_CreateByOperatorWithoutMessage_Expect_DoNotSendMessage()
        {
            ClearServices();

            var testData = new TestData();
            testData.Message = null;
            testData.Initiator = new OperatorInitiator(testData.OperatorId);
            var ticket = await MakeTicket(testData);

            Assert.Catch(() => AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket));
        }

        [Test]
        public async Task When_CreateByOperator_Expect_AssignToCreator()
        {
            ClearServices();

            var testData = new TestData();
            testData.Message = null;
            testData.Initiator = new OperatorInitiator(testData.OperatorId);
            var ticket = await MakeTicket(testData);

            var assignEvt = AssertAndGetPublishedEvent<TicketAssignChangedEvent>(ticket);
            Assert.AreEqual(testData.OperatorId, assignEvt.Assignee);
            Assert.IsInstanceOf<SystemInitiator>(assignEvt.Initiator);
        }

        [Test]
        public async Task When_CreateByOperator_Expect_SetHardAssigment()
        {
            ClearServices();

            var testData = new TestData();
            testData.Message = null;
            testData.Initiator = new OperatorInitiator(testData.OperatorId);
            var ticket = await MakeTicket(testData);

            var hardAssignEvt = AssertAndGetPublishedEvent<TicketAssingmentBindingChangedEvent>(ticket);
            Assert.True(hardAssignEvt.HardAssigment);
            Assert.IsInstanceOf<SystemInitiator>(hardAssignEvt.Initiator);

            Assert.Catch(() => AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket));
        }

        [Test]
        public async Task When_CreateByOperator_Expect_SetPendingAnsweredStatus()
        {
            ClearServices();

            var testData = new TestData();
            testData.Message = null;
            testData.Initiator = new OperatorInitiator(testData.OperatorId);
            var ticket = await MakeTicket(testData);

            var evt = AssertAndGetPublishedEvent<TicketCreatedEvent>(ticket);
            Assert.AreEqual(TicketStatusKind.Pending, evt.Status.Kind);
            Assert.AreEqual(TicketStatusType.Answered, evt.Status.Type);
            AssertEventsNotPublished<TicketStatusChangedEvent>(ticket);
        }

        [Test]
        public async Task When_CreateByOperatorWithMessage_Expect_SendMessage()
        {
            ClearServices();

            var testData = new TestData();
            testData.Initiator = new OperatorInitiator(testData.OperatorId);
            var ticket = await MakeTicket(testData);

            var outgoingMsgEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.AreEqual(testData.Message, outgoingMsgEvt.Message);
            Assert.IsInstanceOf<OperatorInitiator>(outgoingMsgEvt.Initiator);
        }

        [Test]
        public async Task When_CreateWithoutInitiator_Expect_BrokeInitiatorMustExistRule()
        {
            ClearServices();

            var testData = new TestData();
            testData.Initiator = null!;
            AssertBrokenRule<InitiatorMustExistRule>(async () =>
                await MakeTicket(testData));
        }

        [Test]
        public async Task When_CreateByOperatorWithAutoreply_Expect_DoNotSendAutoreply()
        {
            ClearServices();

            ServiceProvider.Autoreplies.ClearSubstitute();
            ServiceProvider.Autoreplies.Get(default).ReturnsForAnyArgs(
                Task.FromResult(new Autoreply(
                    "test",
                    new Message("autoreply"),
                    new[] {new Tag("autoreply")},
                    true)));
            var testData = new TestData();
            testData.Initiator = new OperatorInitiator(testData.OperatorId);
            var ticket = await MakeTicket(testData);
            Assert.Catch<EventNotPublishedException<TicketAutoreplySendedEvent>>(() => AssertAndGetPublishedEvent<TicketAutoreplySendedEvent>(ticket));
        }

        [Test]
        public async Task When_CreationWithBan_Expect_BrokeBunRule()
        {
            ClearServices();

            var testData = new TestData();
            ServiceProvider.Checker.CheckBan(default).ReturnsForAnyArgs(Task.FromResult(true));
            AssertBrokenRule<BanRule>(async () => { await MakeTicket(testData); });
        }

        [Test]
        public async Task When_CreationWithNotExistProject_Expect_BrokeProjectMustExistRule()
        {
            ClearServices();

            var testData = new TestData();
            ServiceProvider.Checker.ProjectIsExist(default).ReturnsForAnyArgs(Task.FromResult(false));
            AssertBrokenRule<ProjectMustExistRule>(async () => { await MakeTicket(testData); });
        }
    }
}
