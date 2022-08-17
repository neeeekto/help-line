using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketApprovalOutgoingMessageMustExistRule : IBusinessRule
    {
        private readonly TicketAuditId _auditId;
        private readonly IEnumerable<TicketApprovalReply> _approvalReplies;

        internal TicketApprovalOutgoingMessageMustExistRule(TicketAuditId auditId, IEnumerable<TicketApprovalReply> approvalReplies)
        {
            _auditId = auditId;
            _approvalReplies = approvalReplies;
        }

        public string Message => $"Approve reply {_auditId} not exist or already approved";

        public bool IsBroken() => _approvalReplies.All(x => x.AuditId != _auditId);
    }
}
