using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters.Values;

public sealed class TicketOperatorInitiatorFilterValue : TicketInitiatorFilterValueBase
{
    public IEnumerable<Guid> Ids { get; set; }
}