using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketApprovalStatusDeniedEvent : TicketEventBase
    {
        public TicketAuditId AuditId { get; private set; }
        public Message Message { get; private set; }
        internal TicketApprovalStatusDeniedEvent(TicketId ticketId, TicketAuditId auditId, Initiator initiator, Message message) : base(ticketId, initiator)
        {
            AuditId = auditId;
            Message = message;
        }
    }
}
