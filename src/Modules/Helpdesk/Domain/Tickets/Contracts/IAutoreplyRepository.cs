using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts
{
    public interface IAutoreplyRepository
    {
        Task<Autoreply?> Get(TicketCreatedEvent evt);
    }
}
