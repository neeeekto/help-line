using System;
using System.Collections.ObjectModel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models
{
    public class TicketsDelayConfiguration
    {
        public string ProjectId { get; internal set; }
        public ReadOnlyDictionary<TicketLifeCycleType, TimeSpan> LifeCycleDelay { get; internal set; }
        public TimeSpan InactivityDelay { get; internal set; }
        public TimeSpan FeedbackCompleteDelay { get; internal set; }
    }
}
