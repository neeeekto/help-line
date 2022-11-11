using System;
using System.Collections.Generic;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Notifications
{
    public class TriggerScheduleFireNotification : INotification
    {
        public TimeSpan Interval { get; }
        public IEnumerable<Guid> Scenarios { get; }

        public TriggerScheduleFireNotification(TimeSpan interval, IEnumerable<Guid> scenarios)
        {
            Interval = interval;
            Scenarios = scenarios;
        }
    }
}
