using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    public abstract class ScheduledEventResultView
    {

        public DateTime Date { get; internal set; }
        public InitiatorView Initiator { get; internal set; }
    }

    public class ScheduledEventCanceledResultView : ScheduledEventResultView
    {

    }

    public class ScheduledEventDoneResultView : ScheduledEventResultView
    {

    }
}
