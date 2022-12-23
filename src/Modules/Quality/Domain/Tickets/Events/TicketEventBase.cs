using System;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Events
{
    // { get; private set; } - mongo restriction! Member without setters do not desreialize
    public abstract class TicketEventBase : EventBase<TicketId>
    {
        public Initiator Initiator { get; private set; }
        public DateTime CreateDate { get; private set; }

        internal TicketEventBase(TicketId ticketId, Initiator initiator) : base(ticketId)
        {
            CreateDate = DateTime.UtcNow;
            Initiator = initiator;
        }
    }
}
