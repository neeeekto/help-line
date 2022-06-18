using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search;


public interface ITicketSearchProvider
{
    Task<PagedResult<TicketView>> Find(PageData pageData, TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts);
    Task<IEnumerable<TicketView>> Find(TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts);
    Task<IEnumerable<string>> FindIds(TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts);
    
}
