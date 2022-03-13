using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using HelpLine.Modules.Helpdesk.Domain.UnitTests.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Domain.UnitTests.Tickets
{
    [TestFixture]
    public class TicketTagsTests : TicketTestsBase
    {
        [Test]
        public async Task When_AddNewTag_Expect_Adding()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var tag1 = new Tag("tag1");
            var tag2 = new Tag("tag2");

            await ticket.Execute(new AddTagsTicketCommand(tag1, tag2), ServiceProvider, testData.Initiator);

            var evt = AssertAndGetPublishedEvent<TicketTagsChangedEvent>(ticket);

            Assert.AreEqual(evt.AggregateId, ticket.Id);
            Assert.AreEqual(evt.Initiator, testData.Initiator);
            Assert.IsTrue(evt.Tags.Contains(testData.Tag));
            Assert.IsTrue(evt.Tags.Contains(tag1));
            Assert.IsTrue(evt.Tags.Contains(tag2));
        }

        [Test]
        public async Task When_AddExistTag_Expect_SuccessWithoutEvent()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new AddTagsTicketCommand(testData.Tag), ServiceProvider, new SystemInitiator());
            Assert.Catch<EventNotPublishedException<TicketTagsChangedEvent>>(() =>
                AssertAndGetPublishedEvent<TicketTagsChangedEvent>(ticket));
        }

        [Test]
        public async Task When_RemoveExistTag_Expect_Removing()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var tag1 = new Tag("tag1");
            var tag2 = new Tag("tag2");
            await ticket.Execute(new AddTagsTicketCommand(tag1, tag2), ServiceProvider, new SystemInitiator());
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RemoveTagsTicketCommand(tag2), ServiceProvider, new SystemInitiator());
            var evt = AssertAndGetPublishedEvent<TicketTagsChangedEvent>(ticket);

            Assert.IsTrue(evt.Tags.Count() == 2);
            Assert.IsTrue(evt.Tags.Contains(testData.Tag));
            Assert.IsTrue(evt.Tags.Contains(tag1));

            Assert.IsFalse(evt.Tags.Contains(tag2));
        }

        [Test]
        public async Task When_RemoveNotExistTag_Expect_SuccessWithoutEvent()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var tag1 = new Tag("tag1");
            ticket.ClearUncommittedEvents();

            await ticket.Execute(new RemoveTagsTicketCommand(tag1), ServiceProvider, new SystemInitiator());
            Assert.Catch<EventNotPublishedException<TicketTagsChangedEvent>>(() =>
                AssertAndGetPublishedEvent<TicketTagsChangedEvent>(ticket));
        }

        [Test]
        public async Task When_SetTagWithoutDuplication_Expect_Success()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var tag1 = new Tag("tag1");
            var tag2 = new Tag("tag2");
            await ticket.Execute(new SetTagsTicketCommand(tag1, tag2), ServiceProvider, new SystemInitiator());

            var evt = AssertAndGetPublishedEvent<TicketTagsChangedEvent>(ticket);
            Assert.IsTrue(evt.Tags.Count() == 2);
            Assert.IsTrue(evt.Tags.Contains(tag1));
            Assert.IsTrue(evt.Tags.Contains(tag2));
        }

        [Test]
        public async Task When_SetTagWithDuplication_Expect_SetWithoutDuplication()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var tag = new Tag("tag");
            await ticket.Execute(new SetTagsTicketCommand(tag, tag), ServiceProvider, new SystemInitiator());

            var evt = AssertAndGetPublishedEvent<TicketTagsChangedEvent>(ticket);
            Assert.IsTrue(evt.Tags.Count() == 1);
            Assert.IsTrue(evt.Tags.Contains(tag));
        }

        [Test]
        public async Task AllMethods_NullInitiator_BreakWithInitiatorMustExistRule()
        {
            ClearServices();
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            ticket.ClearUncommittedEvents();

            var tag = new Tag("tag");

            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new SetTagsTicketCommand(tag), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new AddTagsTicketCommand(tag), ServiceProvider, null!));
            AssertBrokenRule<InitiatorMustExistRule>(() =>
                ticket.Execute(new RemoveTagsTicketCommand(tag), ServiceProvider, null!));
        }
    }
}
