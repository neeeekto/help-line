using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class BanSettings
    {
        public string ProjectId { get;  set; }
        public TimeSpan BanDelay { get;  set; }
        public int TicketsCount { get;  set; }
        public TimeSpan Interval { get;  set; }
    }
}
