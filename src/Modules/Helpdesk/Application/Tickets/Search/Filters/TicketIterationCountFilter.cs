namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;

public sealed class TicketIterationCountFilter : TicketFilterBase
{
    public TicketFilterOperators Operator { get; set; }
    public int Value { get; set; }
}