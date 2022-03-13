using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using NSubstitute;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Domain.UnitTests.Tickets
{
    [TestFixture]
    public class TicketUserTests : TicketTestsBase
    {
        [Test]
        public async Task When_AddLinkedId_Expect_Adding()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("id2");
            var channel = new UserChannelState(new Channel("chat"));
            await ticket.Execute(new AddUserIdTicketCommand(newUserId, channel, UserIdType.Linked), ServiceProvider,
                testData.Initiator);

            var evt = AssertAndGetPublishedEvent<TicketUserIdsChangedEvent>(ticket);

            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(testData.Initiator, evt.Initiator);
            Assert.IsTrue(evt.UserIds.Any(x =>
                x.UserId == newUserId && x.Type == UserIdType.Linked && !x.UseForDiscussion));
        }

        [Test]
        public async Task When_AddLinkedIdWithEnabledFlag_Expect_AddedIdNotUseForOutgoingMessage()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("id2");
            var channel = new UserChannelState(new Channel("chat"), true);
            await ticket.Execute(new AddUserIdTicketCommand(newUserId, channel, UserIdType.Linked), ServiceProvider,
                testData.Initiator);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("Bla")), ServiceProvider, new SystemInitiator());

            var msgEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.IsFalse(msgEvt.Recipients.Any(x => x.UserId == newUserId));
        }

        [Test]
        public async Task When_AddMainIdIn_Expect_Adding()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("id2");
            var channel = new UserChannelState(new Channel("chat"), true);
            await ticket.Execute(new AddUserIdTicketCommand(newUserId, channel, UserIdType.Main), ServiceProvider,
                testData.Initiator);

            var evt = AssertAndGetPublishedEvent<TicketUserIdsChangedEvent>(ticket);

            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(testData.Initiator, evt.Initiator);
            Assert.IsTrue(
                evt.UserIds.Any(x => x.UserId == newUserId && x.Type == UserIdType.Main && x.UseForDiscussion));
        }

        /*[Test]
        public async Task When_Unsubscribe_Expect_Success()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var newUserId = new UserId("id2");
            var message = "test";
            var channel = new UserChannelState(new Channel("chat"));
            await ticket.Execute(new AddUserIdTicketCommand(TicketUserService, newUserId, channel, UserIdType.Main, testData.Initiator);
            ticket.ClearUncommittedEvents();

            await ticket.User.Unsubscribe(TicketUserService, newUserId, message);

            var evt = AssertAndGetPublishedEvent<TicketUserUnsubscribedEvent>(ticket);
            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.IsInstanceOf<UserInitiator>(evt.Initiator);
            Assert.AreEqual(newUserId, (evt.Initiator as UserInitiator)?.UserId);
            Assert.AreEqual(newUserId, evt.UserId);
            Assert.AreEqual(message, evt.Message);
        }

        [Test]
        public async Task When_Unsubscribe_Expect_CallUnsubscribeMethodOnUserService()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var newUserId = new UserId("id2");
            var message = "test";
            var channel = new UserChannelState(new Channel("chat"));
            await ticket.Execute(new AddUserIdTicketCommand(TicketUserService, newUserId, channel, UserIdType.Main, testData.Initiator);
            ticket.ClearUncommittedEvents();
            await ticket.User.Unsubscribe(TicketUserService, newUserId, message);
            TicketUserService.Received(1).Unsubscribe(Arg.Is(ticket.Id), Arg.Is(newUserId), Arg.Is(testData.ProjectId), Arg.Is(message));
        }*/

        [Test]
        public async Task When_AddDuplicateId_Expect_BrokeUserIdMusNotExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<UserIdMusNotExistRule>(async () => await ticket.Execute(new AddUserIdTicketCommand(
                testData.UserId,
                new UserChannelState(testData.Channel), UserIdType.Main), ServiceProvider, testData.Initiator));
        }

        [Test]
        public async Task When_AddDuplicateIdWithOtherChannel_Expect_BrokeUserIdMusNotExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<UserIdMusNotExistRule>(async () => await ticket.Execute(new AddUserIdTicketCommand(
                testData.UserId,
                new UserChannelState(new Channel("test")), UserIdType.Main), ServiceProvider, testData.Initiator));
        }

        [Test]
        public async Task When_AddDuplicateChannel_Expect_BrokeUserIdMusNotExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("newUserId");
            await ticket.Execute(new AddUserIdTicketCommand(newUserId,
                new UserChannelState(testData.Channel), UserIdType.Main), ServiceProvider, testData.Initiator);
            var evt = AssertAndGetPublishedEvent<TicketUserIdsChangedEvent>(ticket);
            Assert.IsTrue(evt.UserIds.Any(x => x.UserId == newUserId && x.Channel == testData.Channel));
        }

        [Test]
        public async Task When_AddUnsubscribedId_Expect_BrokeUnsubscribeRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            ServiceProvider.UnsubscribeManager.CheckExist(default!, default!).ReturnsForAnyArgs(Task.FromResult(true));
            AssertBrokenRule<UnsubscribeRule>(async () => await ticket.Execute(new AddUserIdTicketCommand(
                new UserId("test@te.st"),
                new UserChannelState(testData.Channel), UserIdType.Main), ServiceProvider, testData.Initiator));
        }

        [Test]
        public async Task When_AddMainId_Expect_AddedIdUseForOutgoingMessage()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("id2");
            var channel = new UserChannelState(new Channel("chat"), true);
            await ticket.Execute(new AddUserIdTicketCommand(newUserId, channel, UserIdType.Main), ServiceProvider,
                testData.Initiator);
            ticket.ClearUncommittedEvents();
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("Bla")), ServiceProvider, new SystemInitiator());

            var msgEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.IsTrue(msgEvt.Recipients.Any(x => x.UserId == newUserId && x.Channel == channel.Channel));
        }

        [Test]
        public async Task When_AddLinkedIdInCloseTicket_Expect_Adding()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new RejectTicketCommand(), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("id2");
            var channel = new UserChannelState(new Channel("chat"));
            await ticket.Execute(new AddUserIdTicketCommand(newUserId, channel, UserIdType.Linked), ServiceProvider,
                testData.Initiator);

            var evt = AssertAndGetPublishedEvent<TicketUserIdsChangedEvent>(ticket);

            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(testData.Initiator, evt.Initiator);
            Assert.IsTrue(evt.UserIds.Any(x =>
                x.UserId == newUserId && x.Type == UserIdType.Linked && !x.UseForDiscussion));
        }

        [Test]
        public async Task When_AddMainIdInCloseTicket_Expect_Adding()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            await ticket.Execute(new RejectTicketCommand(), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("id2");
            var channel = new UserChannelState(new Channel("chat"));
            await ticket.Execute(new AddUserIdTicketCommand(newUserId, channel, UserIdType.Linked), ServiceProvider,
                testData.Initiator);

            var evt = AssertAndGetPublishedEvent<TicketUserIdsChangedEvent>(ticket);

            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.AreEqual(testData.Initiator, evt.Initiator);
            Assert.IsTrue(evt.UserIds.Any(x =>
                x.UserId == newUserId && x.Type == UserIdType.Linked && !x.UseForDiscussion));
        }

        [Test]
        public async Task When_RemoveLinkedId_Expect_Removing()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("id2");
            var channel = new UserChannelState(new Channel("chat"));
            await ticket.Execute(new AddUserIdTicketCommand(newUserId, channel, UserIdType.Linked), ServiceProvider,
                testData.Initiator);
            ticket.ClearUncommittedEvents();

            ticket.Execute(new RemoveUserIdTicketCommand(newUserId), ServiceProvider, new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketUserIdsChangedEvent>(ticket);

            Assert.IsFalse(evt.UserIds.Any(x => x.UserId == newUserId));
        }

        [Test]
        public async Task When_RemoveMainId_Expect_Removing()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("id2");
            var channel = new UserChannelState(new Channel("chat"));
            await ticket.Execute(new AddUserIdTicketCommand(newUserId, channel, UserIdType.Main), ServiceProvider,
                testData.Initiator);
            ticket.ClearUncommittedEvents();

            ticket.Execute(new RemoveUserIdTicketCommand(newUserId), ServiceProvider, new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketUserIdsChangedEvent>(ticket);

            Assert.IsFalse(evt.UserIds.Any(x => x.UserId == newUserId));
        }

        [Test]
        public async Task When_RemoveMainId_Expect_RemovedIdNotUseForOutgoingMessages()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("id2");
            var channel = new UserChannelState(new Channel("chat"));
            await ticket.Execute(new AddUserIdTicketCommand(newUserId, channel, UserIdType.Main), ServiceProvider,
                testData.Initiator);
            ticket.ClearUncommittedEvents();
            ticket.Execute(new RemoveUserIdTicketCommand(newUserId), ServiceProvider, new SystemInitiator());
            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("test")), ServiceProvider, new SystemInitiator());

            var outgoingEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.IsFalse(outgoingEvt.Recipients.Any(x => x.UserId == newUserId));
        }

        [Test]
        public async Task When_ToggleChanelOnLinkedId_Expect_BreakWithUserIdTypeMustBeAsRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("id2");
            var channel = new UserChannelState(new Channel("chat"));
            await ticket.Execute(new AddUserIdTicketCommand(newUserId, channel, UserIdType.Linked), ServiceProvider,
                testData.Initiator);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<UserIdTypeMustBeAsRule>(async () =>
            {
                await ticket.Execute(new ToggleUserChannelTicketCommand(newUserId, true), ServiceProvider,
                    new SystemInitiator());
            });
        }

        [Test]
        public async Task When_ToggleChanelOnNotExist_Expect_BrokeUserIdMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<UserIdMustExistRule>(async () =>
            {
                await ticket.Execute(new ToggleUserChannelTicketCommand(new UserId("1212121"), true),
                    ServiceProvider, new SystemInitiator());
            });
        }

        [Test]
        public async Task When_EnableForUnsubscribeId_Expect_BrokeUnsubscribeRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var userId = new UserId("test@te.st");
            await ticket.Execute(new AddUserIdTicketCommand(userId, new UserChannelState(new Channel("test"), false),
                    UserIdType.Main), ServiceProvider,
                new SystemInitiator());

            ticket.ClearUncommittedEvents();
            ServiceProvider.UnsubscribeManager.CheckExist(default!, default!).ReturnsForAnyArgs(Task.FromResult(true));
            AssertBrokenRule<UnsubscribeRule>(async () =>
            {
                await ticket.Execute(new ToggleUserChannelTicketCommand(userId, true), ServiceProvider,
                    new SystemInitiator());
            });
        }

        [Test]
        public async Task When_DisableMainIdChanel_Expect_DisabledIdDoNotUseForOutgoingMessage()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("id2");
            var channel = new UserChannelState(new Channel("chat"));
            await ticket.Execute(new AddUserIdTicketCommand(newUserId, channel, UserIdType.Main), ServiceProvider,
                testData.Initiator);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new ToggleUserChannelTicketCommand(newUserId, false), ServiceProvider,
                new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketUserIdsChangedEvent>(ticket);

            Assert.IsTrue(evt.UserIds.Any(x => x.UserId == newUserId && !x.UseForDiscussion));
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddOutgoingMessageTicketCommand(
                new Message("test")), ServiceProvider, new SystemInitiator());

            var outgoingEvt = AssertAndGetPublishedEvent<TicketOutgoingMessageAddedEvent>(ticket);
            Assert.IsFalse(outgoingEvt.Recipients.Any(x => x.UserId == newUserId));
        }

        [Test]
        public async Task AllMethods_WithoutInitiator_BreakWithInitiatorMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var newUserId = new UserId("id2");
            var channel = new UserChannelState(new Channel("chat"));

            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new AddUserIdTicketCommand(newUserId, channel, UserIdType.Main), ServiceProvider,
                    null!));
            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new RemoveUserIdTicketCommand(newUserId), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(async () =>
                await ticket.Execute(new ToggleUserChannelTicketCommand(newUserId, true), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new ChangeUserMetaTicketCommand(testData.UserMeta), ServiceProvider, null!));
        }

        [Test]
        public async Task When_ChangeMeat_Expect_Success()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var newMeta = new UserMeta(new Dictionary<string, string> {{"new", "new"}});

            await ticket.Execute(new ChangeUserMetaTicketCommand(newMeta), ServiceProvider, testData.Initiator);
            var evt = AssertAndGetPublishedEvent<TicketUserMetaChangedEvent>(ticket);
            Assert.IsTrue(evt.Meta.ContainsKey("new"));
            Assert.IsTrue(evt.Meta.Values.Any(x => x == "new"));
            Assert.AreEqual(evt.Initiator, testData.Initiator);
        }
    }
}
