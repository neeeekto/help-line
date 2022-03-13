using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketReminderCanceledEvent : TicketEventBase
    {
        public TicketReminderId ReminderId { get; private set; }

        internal TicketReminderCanceledEvent(TicketId ticketId, Initiator initiator, TicketReminderId reminderId) : base(ticketId, initiator)
        {
            ReminderId = reminderId;
        }
    }
}
