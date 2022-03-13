using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Operators
{
    public class OperatorId : TypedGuidIdValueBase
    {
        public OperatorId(Guid value) : base(value)
        {
        }
    }
}
