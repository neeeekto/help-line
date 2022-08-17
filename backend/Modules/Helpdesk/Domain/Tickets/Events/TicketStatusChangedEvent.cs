using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketStatusChangedEvent : TicketEventBase
    {
        public TicketStatus Status { get; private set; }

        public TicketStatusChangedEvent(TicketId ticketId, Initiator initiator, TicketStatus status) : base(ticketId, initiator)
        {
            Status = status;
        }
    }
}
