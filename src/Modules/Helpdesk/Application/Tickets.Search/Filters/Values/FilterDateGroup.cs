using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters.Values;

public sealed class FilterDateGroup : FilterDateValueBase
{
    public bool Intersection { get; set; }
    public IEnumerable<FilterDateValue> Values { get; set; }
}