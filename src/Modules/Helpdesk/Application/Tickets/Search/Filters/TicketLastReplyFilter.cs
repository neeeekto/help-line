using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters.Values;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketLastReplyFilter : TicketFilterBase
{
    public FilterDateValue Value { get; set; }
}
