using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketUserIdsChangedEventView : TicketEventView
    {
        public IEnumerable<UserIdInfoView> Old { get; internal set; }
        public IEnumerable<UserIdInfoView> New { get; internal set; }
    }
}
