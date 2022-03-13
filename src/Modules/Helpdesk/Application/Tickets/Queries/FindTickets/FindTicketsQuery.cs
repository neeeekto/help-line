using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Queries.FindTickets
{
    public class FindTicketsQuery : QueryBase<PagedResult<TicketView>>
    {
        public PageData PageData { get; }
        public IFilter Filter { get; }
        public IEnumerable<Sort> Sorts { get; }

        public FindTicketsQuery(PageData pageData, IFilter filter, IEnumerable<Sort> sorts)
        {
            PageData = pageData;
            Filter = filter;
            Sorts = sorts;
        }
    }
}
