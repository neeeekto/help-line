using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketClosedEvent : TicketEventBase
    {
        internal TicketClosedEvent(TicketId ticketId, Initiator initiator) : base(ticketId, initiator)
        {
        }
    }
}
