using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;

public sealed class TicketProjectFilter : TicketFilterBase
{
    public IEnumerable<string> Value { get; set; }
}