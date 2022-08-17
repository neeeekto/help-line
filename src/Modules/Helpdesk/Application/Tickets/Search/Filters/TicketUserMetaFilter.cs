using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketUserMetaFilter : TicketFilterBase
{
    public string Key { get; set; }
    public IEnumerable<string> Value { get; set; }
}