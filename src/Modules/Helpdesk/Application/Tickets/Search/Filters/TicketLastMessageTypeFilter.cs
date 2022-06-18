using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

public sealed class TicketLastMessageTypeFilter : TicketFilterBase
{
    public TicketDiscussionStateView.MessageType Value { get; set; }
}