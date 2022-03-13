using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketStatusChangedEventView : TicketEventView
    {
        public TicketStatusView Old { get; internal set; }
        public TicketStatusView New { get; internal set; }
    }
}
