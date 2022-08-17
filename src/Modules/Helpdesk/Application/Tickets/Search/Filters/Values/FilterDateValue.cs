using System;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters.Values;

public sealed class FilterDateValue : FilterDateValueBase
{
    public TicketFilterOperators Operator { get; set; }
    public DateTime? DateTime { get; set; } // null - now
    public FilterDateValueAction? Action { get; set; } // null - no actions
}

public class FilterDateValueAction
{
    public enum Operations
    {
        Add,
        Sub
    }
    
    public Operations Operation { get; set; }
    public TimeSpan Amount { get; set; }
}
