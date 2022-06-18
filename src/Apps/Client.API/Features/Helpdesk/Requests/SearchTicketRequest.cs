using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Requests
{
    public class SearchTicketRequest
    {
        public TicketFilterBase? Filter { get; set; }
        public IEnumerable<TicketSortBase> Sorts { get; set; }
    }
}
