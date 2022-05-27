using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;

public sealed class TicketMetaFilter : TicketFilterBase
{
    public IEnumerable<string>? FromTickets { get; internal set; }
    public IEnumerable<string>? Sources { get; internal set; }
    public IEnumerable<string>? Platforms { get; internal set; }
}