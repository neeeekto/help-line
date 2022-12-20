using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketTagsChangedEventView : TicketEventView
    {
        public IEnumerable<string> Old { get; internal set; }
        public IEnumerable<string> New { get; internal set; }
    }
}
