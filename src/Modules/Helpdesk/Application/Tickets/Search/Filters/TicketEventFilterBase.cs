using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters.Values;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;

public abstract class TicketEventFilterBase : TicketFilterBase
{
    public IEnumerable<TicketInitiatorFilterValueBase?> Initiators { get; set; }
    public TicketFilterDateValue? CreateDate { get; set; }
}
