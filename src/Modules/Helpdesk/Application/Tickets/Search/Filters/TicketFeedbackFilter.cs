using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters.Values;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;

public sealed class TicketFeedbackFilter : TicketFilterBase
{
    public TicketFilterDateValue? Date { get; set; }
    public bool? Solved { get; set; }
    public IEnumerable<int>? Scores { get; set; }
    public IDictionary<string, IEnumerable<int>>? OptionalScores { get; set; }
}
