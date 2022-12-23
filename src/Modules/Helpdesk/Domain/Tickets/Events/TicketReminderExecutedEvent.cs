using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketReminderExecutedEvent : TicketEventBase
    {
        public TicketReminderId ReminderId { get; private set; }
        internal TicketReminderExecutedEvent(TicketId ticketId, Initiator initiator, TicketReminderId reminderId) : base(ticketId, initiator)
        {
            ReminderId = reminderId;
        }
    }
}
