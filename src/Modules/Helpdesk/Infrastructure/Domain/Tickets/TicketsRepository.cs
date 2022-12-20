using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using HelpLine.BuildingBlocks.Infrastructure.EventSourcing;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets
{
    internal class TicketsRepository : EventsSourcingAggregateRepository<TicketId, Ticket, TicketState>,
        ITicketsRepository
    {
        private readonly IMongoContext _context;

        public TicketsRepository(IEventStore<TicketId> eventStore, ISnapshotStore<TicketId, TicketState> snapshotStore,
            IDomainEventCollector eventCollector, IMongoContext context) : base(
            30, eventStore, snapshotStore, eventCollector)
        {
            _context = context;
        }

        async Task<IEnumerable<Ticket>> ITicketsRepository.GetByUserId(UserId userId, ProjectId projectId,
            CancellationToken cancellationToken)
        {
            var userTickets = await _context.GetCollection<TicketView>()
                .Find(x => x.ProjectId == projectId.Value && x.UserIds.Any(x =>
                    x.UserId == userId.Value && x.Type == UserIdType.Main && x.UseForDiscussion))
                .ToListAsync(cancellationToken);
            if (userTickets == null)
                return new Ticket[] { };

            var tickets =
                await Task.WhenAll(userTickets.Select(x => GetByIdAsync(new TicketId(x.Id), cancellationToken)));
            return tickets;
        }
    }
}
