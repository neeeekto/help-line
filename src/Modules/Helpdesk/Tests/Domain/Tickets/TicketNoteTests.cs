using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Tests.Domain.Tickets
{
    [TestFixture]
    public class TicketNoteTests : TicketTestsBase
    {
        [Test]
        public async Task When_AddNoteWithCorrectMessage_Expect_Adding()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var noteTag = "pinned";
            await ticket.Execute(new AddPrivateNoteTicketCommand(testData.Message, new[] {noteTag}), ServiceProvider,
                testData.Initiator);

            var evt = AssertAndGetPublishedEvent<TicketNotePostedEvent>(ticket);
            Assert.AreEqual(evt.AggregateId, ticket.Id);
            Assert.AreEqual(evt.Message, testData.Message);
            Assert.IsTrue(evt.Tags.Contains(noteTag));
            Assert.IsNotNull(evt.NoteId);
        }

        [Test]
        public async Task When_AddNoteWithEmptyMessage_Expect_BreakWithMessageCannotBeEmptyRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<MessageCannotBeEmptyRule>(() =>
                ticket.Execute(new AddPrivateNoteTicketCommand(null!), ServiceProvider, testData.Initiator));
            AssertBrokenRule<MessageCannotBeEmptyRule>(() =>
                ticket.Execute(new AddPrivateNoteTicketCommand(new Message("")!), ServiceProvider, testData.Initiator));
        }

        [Test]
        public async Task When_ChangeNoteMessageOnCorrectMessage_Expect_Changing()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddPrivateNoteTicketCommand(testData.Message), ServiceProvider,
                testData.Initiator);
            var addEvt = AssertAndGetPublishedEvent<TicketNotePostedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            var newMessage = new Message("new note");
            var newTag = "pinned";
            await ticket.Execute(new ChangePrivateNoteTicketCommand(addEvt.NoteId, newMessage, new[] {newTag}),
                ServiceProvider, testData.Initiator);

            var evt = AssertAndGetPublishedEvent<TicketNotePostedEvent>(ticket);
            Assert.AreEqual(evt.AggregateId, ticket.Id);
            Assert.AreEqual(evt.Message, newMessage);
            Assert.AreEqual(evt.NoteId, addEvt.NoteId);
            Assert.IsTrue(evt.Tags.Contains(newTag));
        }

        [Test]
        public async Task When_ChangeNoteMessageOnEmptyMessage_Expect_BreakWithMessageCannotBeEmptyRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddPrivateNoteTicketCommand(testData.Message), ServiceProvider,
                testData.Initiator);
            var addEvt = AssertAndGetPublishedEvent<TicketNotePostedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<MessageCannotBeEmptyRule>(() =>
                ticket.Execute(new ChangePrivateNoteTicketCommand(addEvt.NoteId, null!), ServiceProvider,
                    testData.Initiator));
            AssertBrokenRule<MessageCannotBeEmptyRule>(() =>
                ticket.Execute(new ChangePrivateNoteTicketCommand(addEvt.NoteId, new Message("")!), ServiceProvider,
                    testData.Initiator));
        }

        [Test]
        public async Task When_ChangeNotExistNote_Expect_BreakWithTicketNoteMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketNoteMustExistRule>(() =>
                ticket.Execute(new ChangePrivateNoteTicketCommand(new TicketNoteId(),
                    testData.Message), ServiceProvider, testData.Initiator));
        }

        [Test]
        public async Task When_RemoveExistNote_Expect_Removing()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddPrivateNoteTicketCommand(testData.Message), ServiceProvider,
                testData.Initiator);
            var addEvt = AssertAndGetPublishedEvent<TicketNotePostedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RemovePrivateNoteTicketCommand(addEvt.NoteId), ServiceProvider,
                testData.Initiator);

            var evt = AssertAndGetPublishedEvent<TicketNoteRemovedEvent>(ticket);
            Assert.AreEqual(evt.AggregateId, ticket.Id);
            Assert.AreEqual(evt.NoteId, addEvt.NoteId);
        }

        [Test]
        public async Task When_RemoveNotExistNote_Expect_BreakWithTicketNoteMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<TicketNoteMustExistRule>(() =>
                ticket.Execute(new RemovePrivateNoteTicketCommand(new TicketNoteId()), ServiceProvider,
                    testData.Initiator));
        }

        [Test]
        public async Task When_ChangeTagsNoneExistentNote_Expect_BreakWithTicketNoteMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();
            AssertBrokenRule<TicketNoteMustExistRule>(() =>
                ticket.Execute(new ChangePrivateNoteTagsTicketCommand(new TicketNoteId(), new string[] { }),
                    ServiceProvider, testData.Initiator));
        }

        [Test]
        public async Task When_ChangeTagsExistentNote_Expect_Changing()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var oldTag = "old";
            await ticket.Execute(new AddPrivateNoteTicketCommand(testData.Message, new[] {oldTag}), ServiceProvider,
                testData.Initiator);
            var addEvt = AssertAndGetPublishedEvent<TicketNotePostedEvent>(ticket);
            ticket.ClearUncommittedEvents();

            var newTag = "new";
            await ticket.Execute(new ChangePrivateNoteTagsTicketCommand(addEvt.NoteId, new[] {newTag}), ServiceProvider,
                testData.Initiator);

            var evt = AssertAndGetPublishedEvent<TicketNotePostedEvent>(ticket);
            Assert.IsTrue(evt.Tags.Contains(newTag));
            Assert.IsFalse(evt.Tags.Contains(oldTag));
        }

        [Test]
        public async Task When_AddNoteWithDuplicateTags_Expect_AddingNoteWithoutDuplicationTags()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddPrivateNoteTicketCommand(testData.Message,
                new[] {"test", "test", "test"}), ServiceProvider, testData.Initiator);
            var evt = AssertAndGetPublishedEvent<TicketNotePostedEvent>(ticket);
            Assert.IsTrue(evt.Tags.Contains("test"));
            Assert.IsTrue(evt.Tags.Count() == 1);
        }

        [Test]
        public async Task When_EditRemovedNote_Expect_BreakWithTicketNoteMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddPrivateNoteTicketCommand(testData.Message), ServiceProvider,
                testData.Initiator);
            var evt = AssertAndGetPublishedEvent<TicketNotePostedEvent>(ticket);
            await ticket.Execute(new RemovePrivateNoteTicketCommand(evt.NoteId), ServiceProvider, testData.Initiator);

            AssertBrokenRule<TicketNoteMustExistRule>(() =>
                ticket.Execute(new ChangePrivateNoteTicketCommand(evt.NoteId, testData.Message), ServiceProvider,
                    testData.Initiator));
        }

        [Test]
        public async Task AllMethods_WithoutInitiator_BreakWithInitiatorMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new AddPrivateNoteTicketCommand(testData.Message), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new ChangePrivateNoteTicketCommand(new TicketNoteId(), testData.Message),
                    ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new ChangePrivateNoteTagsTicketCommand(new TicketNoteId(), new string[] { }),
                    ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new RemovePrivateNoteTicketCommand(new TicketNoteId()), ServiceProvider, null!));
        }
    }
}
