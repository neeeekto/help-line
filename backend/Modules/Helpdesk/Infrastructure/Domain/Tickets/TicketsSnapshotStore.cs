using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.EventSourcing;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets
{
    internal class TicketsSnapshotStore : ShanpshotStore<TicketId, TicketState>
    {
        public TicketsSnapshotStore(IMongoContext context) : base(context)
        {
        }
    }
}
