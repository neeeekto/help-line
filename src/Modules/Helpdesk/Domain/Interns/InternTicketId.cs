using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Interns
{
    public class InternTicketId : TypedGuidIdValueBase
    {
        public InternTicketId(Guid value) : base(value)
        {
        }

        internal InternTicketId() : base(Guid.NewGuid())
        {
        }
    }
}
