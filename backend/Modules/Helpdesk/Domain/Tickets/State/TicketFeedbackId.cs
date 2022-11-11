using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class TicketFeedbackId : TypedGuidIdValueBase
    {
        public TicketFeedbackId(Guid value) : base(value)
        {
        }
        
        internal TicketFeedbackId() : base(Guid.NewGuid())
        {
        }
    }
}
