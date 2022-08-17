using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems
{
    public class TemporaryProblemUpdateId : TypedGuidIdValueBase
    {
        public TemporaryProblemUpdateId(Guid value) : base(value)
        {
        }

        internal TemporaryProblemUpdateId() : base(Guid.NewGuid())
        {
        }
    }
}
