using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketProjectFilter : TicketFilterBase
{
    public IEnumerable<string> Value { get; set; }
}