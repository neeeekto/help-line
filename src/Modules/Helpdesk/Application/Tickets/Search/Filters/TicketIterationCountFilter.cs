namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketIterationCountFilter : TicketFilterBase
{
    public TicketFilterOperators Operator { get; set; }
    public int Value { get; set; }
}