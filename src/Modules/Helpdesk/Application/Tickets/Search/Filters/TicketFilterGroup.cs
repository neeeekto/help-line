using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;


public class TicketFilterGroup : TicketFilterBase
{
    public bool Intersection { get; set; } // true = and, false = or
    public IEnumerable<TicketFilterBase> Filters { get; set; }
}
