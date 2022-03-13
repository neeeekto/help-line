using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using NSubstitute;
using NUnit.Framework;

namespace HelpLine.Modules.Helpdesk.Domain.UnitTests.Tickets
{
    [TestFixture]
    public class TicketsManagerTests : TicketTestsBase
    {
        [SetUp]
        public void Setup()
        {
            ClearServices();
        }

        [Test]
        public async Task Where_UnsubscribeForExist_Expect_Success()
        {
            var testData = new TestData();
            var ticket = await MakeTicket(testData);
            var message = "test";
            ticket.ClearUncommittedEvents();
            var repository = new Repository(ticket);
            var ticketsManager = new TicketsService(repository, ServiceProvider);

            await ticketsManager.Unsubscribe(testData.UserId, testData.ProjectId, message);

            var evt = AssertAndGetPublishedEvent<TicketUserUnsubscribedEvent>(ticket);
            Assert.AreEqual(ticket.Id, evt.AggregateId);
            Assert.IsInstanceOf<UserInitiator>(evt.Initiator);
            Assert.AreEqual(testData.UserId, (evt.Initiator as UserInitiator)?.UserId);
            Assert.AreEqual(testData.UserId, evt.UserId);
            Assert.AreEqual(message, evt.Message);
            Assert.IsTrue(repository.Saved.Any(x => x.Id == ticket.Id));
        }

        [Test]
        public async Task Where_UnsubscribeForNotExist_Expect_NoActions()
        {
            var testData = new TestData();
            var repository = new Repository();
            var ticketsManager = new TicketsService(repository, ServiceProvider);

            await ticketsManager.Unsubscribe(testData.UserId, testData.ProjectId, "");
            Assert.AreEqual(0, repository.Saved.Count);
        }

        [Test]
        public async Task Where_UnsubscribeForFewTickets_Expect_Success()
        {
            var testData = new TestData();
            var ticket1 = await MakeTicket(testData);

            ServiceProvider.IdFactory.GetNext(default).ReturnsForAnyArgs(Task.FromResult(new TicketId("0-000001")));
            var ticket2 = await MakeTicket(testData);

            var repository = new Repository(ticket1, ticket2);
            var ticketsManager = new TicketsService(repository, ServiceProvider);

            await ticketsManager.Unsubscribe(testData.UserId, testData.ProjectId, "");
            Assert.AreEqual(2, repository.Saved.Count);

            AssertEventsPublished<TicketUserUnsubscribedEvent>(ticket1);
            AssertEventsPublished<TicketUserUnsubscribedEvent>(ticket2);
        }
    }

    class Repository : ITicketsRepository
    {
        private readonly IEnumerable<Ticket> _tickets;
        public List<Ticket> Saved = new List<Ticket>();

        public Repository(params Ticket[] tickets)
        {
            _tickets = tickets;
        }

        public Task SaveAsync(Ticket aggregate, CancellationToken cancellationToken = default)
        {
            Saved.Add(aggregate);
            return Task.CompletedTask;
        }

        public Task<Ticket> GetByIdAsync(TicketId id, CancellationToken cancellationToken = default)
        {
            return null;
        }

        Task<IEnumerable<Ticket>> ITicketsRepository.GetByUserId(UserId userId, ProjectId projectId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_tickets);
        }
    }
}
