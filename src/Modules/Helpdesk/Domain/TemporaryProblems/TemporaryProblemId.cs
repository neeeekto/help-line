using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.TemporaryProblems
{
    public class TemporaryProblemId : TypedGuidIdValueBase
    {
        public TemporaryProblemId(Guid value) : base(value)
        {
        }

        internal TemporaryProblemId() : base(Guid.NewGuid())
        {
        }
    }
}
