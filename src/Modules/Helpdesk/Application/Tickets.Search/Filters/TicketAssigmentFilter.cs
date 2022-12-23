using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketAssigmentFilter : TicketFilterBase
{
    public IEnumerable<ValueBase> Values { get; set; }
    
    public abstract record ValueBase {}
    public sealed record CurrentOperator : ValueBase {}
    public sealed record Unassigned : ValueBase {}
    public sealed record Operator : ValueBase
    {
        public Guid Id { get; init; }
    }
}
