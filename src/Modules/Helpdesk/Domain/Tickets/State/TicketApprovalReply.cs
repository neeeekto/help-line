namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class TicketApprovalReply
    {
        public TicketAuditId AuditId { get; }
        public Message Message { get; internal set; }
        public Initiator Initiator { get; }

        internal TicketApprovalReply(TicketAuditId auditId, Message message, Initiator initiator)
        {
            AuditId = auditId;
            Message = message;
            Initiator = initiator;
        }
    }
}
