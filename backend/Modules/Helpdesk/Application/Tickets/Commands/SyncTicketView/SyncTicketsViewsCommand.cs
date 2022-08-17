using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.SyncTicketView;

public class SyncTicketsViewsCommand : CommandBase
{
    public TicketFilterBase? Filter { get; }

    public SyncTicketsViewsCommand(TicketFilterBase? filter)
    {
        Filter = filter;
    }
}
