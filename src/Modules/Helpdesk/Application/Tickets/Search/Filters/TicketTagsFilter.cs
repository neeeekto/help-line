using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketTagsFilter : TicketFilterBase
{
    public bool Exclude { get; set; } // false = exclude, true = include
    public IEnumerable<string> Tags { get; set; }
}
