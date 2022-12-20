using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketEventExistFilter : TicketEventFilterBase
{
    public IEnumerable<string> Types { get; set; } // key of event. We need support map between client and server
}
