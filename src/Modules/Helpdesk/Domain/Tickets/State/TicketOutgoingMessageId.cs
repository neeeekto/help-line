using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class TicketOutgoingMessageId : TypedGuidIdValueBase
    {
        public TicketOutgoingMessageId(Guid value) : base(value)
        {
        }

        internal TicketOutgoingMessageId() : base(Guid.NewGuid()) {}
    }
}
