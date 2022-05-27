using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters.Values;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;

public sealed class FilterDateGroup : FilterDateValueBase
{
    public bool Intersection { get; set; }
    public IEnumerable<FilterDateValue> Values { get; set; }
}