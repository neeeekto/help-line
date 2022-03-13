using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketApprovalStatusAddedEvent : TicketEventBase
    {
        public Message Message { get; private set; }
        public TicketAuditId AuditId { get; private set; }
        public TicketStatus Status { get; private set; }
        internal TicketApprovalStatusAddedEvent(TicketId ticketId, Initiator initiator, Message message, TicketAuditId auditId, TicketStatus status) : base(ticketId, initiator)
        {
            Message = message;
            AuditId = auditId;
            Status = status;
        }
    }
}
