using System;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class ScheduleId : TypedGuidIdValueBase
    {
        public ScheduleId(Guid value) : base(value)
        {
        }

        internal ScheduleId() : base(Guid.NewGuid())
        {
            
        }
    }
}
