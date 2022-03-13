using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Interns
{
    public class InternHandlingVersionId : TypedGuidIdValueBase
    {
        public InternHandlingVersionId(Guid value) : base(value)
        {
        }

        internal InternHandlingVersionId() : base(Guid.NewGuid())
        {
        }
    }
}
