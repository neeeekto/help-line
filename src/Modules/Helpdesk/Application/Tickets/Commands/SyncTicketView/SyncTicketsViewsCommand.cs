using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.RecreateTicketView;

public class SyncTicketsViewsCommand : CommandBase
{
    public TicketFilterBase? Filter { get; }

    public SyncTicketsViewsCommand(TicketFilterBase? filter)
    {
        Filter = filter;
    }
}
