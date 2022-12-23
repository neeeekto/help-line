using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketUserMetaChangedEventView : TicketEventView
    {
        public IDictionary<string, string> Old { get; internal set; }
        public IDictionary<string, string> New { get; internal set; }
    }
}
