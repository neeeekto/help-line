using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;

public sealed class TicketStatusFilter : TicketFilterBase
{
    public TicketStatusKind? Kind { get; set; }
    public IEnumerable<TicketStatusType> Type { get; set; }
}