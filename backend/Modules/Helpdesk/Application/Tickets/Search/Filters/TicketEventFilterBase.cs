using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters.Values;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public abstract class TicketEventFilterBase : TicketFilterBase
{
    public IEnumerable<TicketInitiatorFilterValueBase>? Initiators { get; set; }
    public FilterDateValueBase? CreateDate { get; set; }
}
