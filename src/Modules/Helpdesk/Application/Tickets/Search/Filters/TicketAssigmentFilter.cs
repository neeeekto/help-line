using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketAssigmentFilter : TicketFilterBase
{
    public IEnumerable<ValueBase> Values { get; set; }
    
    public abstract class ValueBase {}
    public sealed class CurrentOperator : ValueBase {}
    public sealed class Unassigned : ValueBase {}
    public sealed class Operator : ValueBase
    {
        public Guid Id { get; set; }
    }
}
