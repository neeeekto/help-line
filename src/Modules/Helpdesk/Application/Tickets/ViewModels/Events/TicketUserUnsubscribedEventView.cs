namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketUserUnsubscribedEventView : TicketEventView
    {
        public string UserId { get; internal set; }
        public string Message { get; internal set; }
    }
}
