namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters.Values;

public sealed class FilterDateValue : FilterDateValueBase
{
    public TicketFilterOperators Operator { get; set; }
    public string Expression { get; set; }
}
