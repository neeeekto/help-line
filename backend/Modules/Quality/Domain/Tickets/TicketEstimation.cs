using System;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.Modules.Quality.Domain.Tickets.Events;
using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets
{
    public class TicketEstimation : TicketManagerBase
    {
        internal TicketEstimation(Ticket ticket, Action<EventBase<TicketId>> riseEvent) : base(ticket, riseEvent)
        {
        }

        public void Add(ManagerInitiator manager, string key, double value, bool isNormal)
        {
            RiseEvent(new TicketEstimatedEvent(Ticket.Id, manager, key, value, isNormal));
        }
    }
}
