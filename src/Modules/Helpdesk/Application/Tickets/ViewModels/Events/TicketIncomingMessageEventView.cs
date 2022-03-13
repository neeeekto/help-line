namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketIncomingMessageEventView : TicketEventView
    {
        public MessageView Message { get; internal set; }
        public string Channel { get; internal set; }
    }
}
