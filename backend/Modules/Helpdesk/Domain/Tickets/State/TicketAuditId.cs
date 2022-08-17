using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class TicketAuditId : TypedGuidIdValueBase
    {
        public TicketAuditId(Guid value) : base(value)
        {
        }

        public TicketAuditId() : base(Guid.NewGuid())
        {
        }

    }
}
