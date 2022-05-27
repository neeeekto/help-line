using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters.Values;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;

public sealed class TicketCreateDateFilter : TicketFilterBase
{
    public FilterDateValue Value { get; set; } // Or/And - use TicketFilterGroup
}
