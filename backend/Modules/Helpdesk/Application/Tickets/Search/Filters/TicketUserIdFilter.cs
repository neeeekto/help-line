using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketUserIdFilter : TicketFilterBase
{
    public UserIdType? Type { get; set; }
    public bool? UseForDiscussion { get; set; }
    public IEnumerable<string>? Channel { get; set; } // Empty = 
    public IEnumerable<string> Ids { get; set; }
}
