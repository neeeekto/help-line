using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.EventSourcing;
using HelpLine.Modules.Helpdesk.Domain.Tickets;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets
{
    internal class TicketsEventStore : EventStore<TicketId>
    {
        public TicketsEventStore(IMongoContext context) : base(context)
        {
        }
    }
}
