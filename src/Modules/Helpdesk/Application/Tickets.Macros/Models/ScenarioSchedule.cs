using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models
{
    public class ScenarioSchedule
    {
        public TimeSpan Interval { get; internal set; }
        public DateTime NextTriggerDate { get; internal set; }
        public IEnumerable<Guid> Scenarios { get; internal set; }

        internal void CalculateAndSetNextTriggerDate()
        {
            NextTriggerDate = DateTime.UtcNow + Interval;
        }
    }
}
