namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketLanguagesChangedEventView : TicketEventView
    {
        public string From { get; internal set; }
        public string To { get; internal set; }
    }
}
