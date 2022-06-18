using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Projectors;

public interface ITicketViewRepository
{
    protected internal Task<TicketView> Get(string ticketId, CancellationToken cancellationToken = default);
    protected internal  Task Save(TicketView data, CancellationToken cancellationToken = default);
}
