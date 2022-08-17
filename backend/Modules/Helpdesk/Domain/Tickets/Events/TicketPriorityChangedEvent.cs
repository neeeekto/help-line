using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketPriorityChangedEvent : TicketEventBase
    {
        public TicketPriority Priority { get; private set; }

        public TicketPriorityChangedEvent(TicketId ticketId, Initiator initiator, TicketPriority priority) : base(ticketId, initiator)
        {
            Priority = priority;
        }
    }
}
