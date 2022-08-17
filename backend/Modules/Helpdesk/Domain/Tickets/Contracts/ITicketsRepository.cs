using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts
{
    public interface ITicketsRepository : IEventsSourcingAggregateRepository<TicketId, Ticket>
    {
        protected internal Task<IEnumerable<Ticket>> GetByUserId(UserId userId, ProjectId projectId, CancellationToken cancellationToken = default);
    }
}
