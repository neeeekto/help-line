using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketAssingmentBindingChangedEvent : TicketEventBase
    {
        public bool HardAssigment { get; private set; }

        internal TicketAssingmentBindingChangedEvent(TicketId ticketId, Initiator initiator, bool hardAssigment) : base(ticketId, initiator)
        {
            HardAssigment = hardAssigment;
        }
    }
}
