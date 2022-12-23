using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketPriorityEventView : TicketEventView
    {
        public TicketPriority Old { get; internal set; }
        public TicketPriority New { get; internal set; }
    }
}
