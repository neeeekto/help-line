using System;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;

namespace HelpLine.Modules.Quality.Domain.Tickets
{
    public abstract class TicketManagerBase : Entity
    {
        protected Ticket Ticket { get; }
        private Action<EventBase<TicketId>> _riseEvent { get; }

        protected TicketManagerBase(Ticket ticket, Action<EventBase<TicketId>> riseEvent)
        {
            Ticket = ticket;
            _riseEvent = riseEvent;
        }


        protected void RiseEvent(EventBase<TicketId> evt)
        {
            _riseEvent(evt);
        }
    }
}
