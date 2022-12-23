using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class TicketReminderId : TypedGuidIdValueBase
    {
        public TicketReminderId(Guid value) : base(value)
        {
        }

        public TicketReminderId() : base(Guid.NewGuid())
        {
        }
    }
}
