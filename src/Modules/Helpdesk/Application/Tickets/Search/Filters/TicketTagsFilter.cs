using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;

public sealed class TicketTagsFilter : TicketFilterBase
{
    public bool Exclude { get; set; } // false = exclude, true = include
    public bool Any { get; set; } // false = and, true = or
    public IEnumerable<string> Tags { get; set; }
}