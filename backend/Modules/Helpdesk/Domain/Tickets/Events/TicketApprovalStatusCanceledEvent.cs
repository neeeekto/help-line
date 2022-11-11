using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketApprovalStatusCanceledEvent: TicketEventBase
    {
        public TicketAuditId AuditId { get; private set; }
        internal TicketApprovalStatusCanceledEvent(TicketId ticketId, TicketAuditId auditId, Initiator initiator) : base(ticketId, initiator)
        {
            AuditId = auditId;
        }
    }
}
