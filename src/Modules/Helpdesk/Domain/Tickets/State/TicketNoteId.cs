using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class TicketNoteId : TypedGuidIdValueBase
    {
        public TicketNoteId(Guid value) : base(value)
        {
        }

        public TicketNoteId() : base(Guid.NewGuid())
        {
        }
    }
}
