using System;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketLifecycleEventView : TicketEventView
    {
        public TicketLifeCycleType Type { get; internal set; }
        public ScheduledEventResultView? Result { get; internal set; }
        public DateTime ExecutionDate { get; internal set; }
    }
}
