using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters.Values;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketFeedbackFilter : TicketFilterBase
{
    public FilterDateValueBase? Date { get; set; }
    public bool? Solved { get; set; }
    public IEnumerable<int> Scores { get; set; }
    public IDictionary<string, IEnumerable<int>> OptionalScores { get; set; }
    public bool? HasMessage { get; set; }

}
