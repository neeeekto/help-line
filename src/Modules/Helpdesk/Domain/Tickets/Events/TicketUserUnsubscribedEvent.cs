using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketUserUnsubscribedEvent : TicketEventBase
    {
        public UserId UserId { get; private set; }
        public string Message { get; private set; }

        internal TicketUserUnsubscribedEvent(TicketId ticketId, UserId userId, string message, Initiator initiator) : base(ticketId,
            initiator)
        {
            UserId = userId;
            Message = message;
        }
    }
}
