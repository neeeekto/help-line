using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters.Values;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketCreateDateFilter : TicketFilterBase
{
    public FilterDateValue Value { get; set; } // Or/And - use TicketFilterGroup
}
