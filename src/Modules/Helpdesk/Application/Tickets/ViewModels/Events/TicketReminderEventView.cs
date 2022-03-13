namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketReminderEventView : TicketEventView
    {
        public ReminderView Reminder { get; internal set; }
        public ScheduledEventResultView? Result { get; internal set; }
    }
}
