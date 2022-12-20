using System;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    // TODO: Add description for { get; private set; } - mongo restriction! Member without setters do not desreialize
    public abstract class TicketEventBase : EventBase<TicketId>
    {
        public Initiator Initiator { get; private set; }
        public DateTime CreateDate { get; private set; }

        internal TicketEventBase(TicketId ticketId, Initiator initiator) : base(ticketId)
        {
            Initiator = initiator;
            CreateDate = DateTime.UtcNow;
        }
    }
}
