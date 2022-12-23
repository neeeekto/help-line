using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search.ElasticSearch;

internal class TicketElasticSearchProvider : ITicketSearchProvider
{
    public Task<PagedResult<TicketView>> Find(PageData pageData, TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<TicketView>> Find(TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<string>> FindIds(TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts)
    {
        throw new System.NotImplementedException();
    }
}
