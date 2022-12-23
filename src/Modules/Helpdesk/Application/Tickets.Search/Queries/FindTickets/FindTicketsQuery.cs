using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Queries.FindTickets
{
    public class FindTicketsQuery : QueryBase<PagedResult<TicketView>>
    {
        public PageData PageData { get; }
        public TicketFilterBase Filter { get; }
        public IEnumerable<TicketSortBase> Sorts { get; }

        public FindTicketsQuery(PageData pageData, TicketFilterBase filter, IEnumerable<TicketSortBase> sorts)
        {
            PageData = pageData;
            Filter = filter;
            Sorts = sorts;
        }
    }
}
