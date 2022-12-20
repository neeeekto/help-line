using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class ApproveTicketRejectAction : TicketActionBase
    {
        public Guid AuditId { get; set; }

        public ApproveTicketRejectAction()
        {
        }

        public ApproveTicketRejectAction(Guid auditId)
        {
            AuditId = auditId;
        }
    }
}
