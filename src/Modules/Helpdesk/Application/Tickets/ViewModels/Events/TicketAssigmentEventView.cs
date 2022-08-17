using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketAssigmentEventView : TicketEventView
    {
        public Guid? From { get; internal set; }
        public Guid? To { get; internal set; }
    }
}
