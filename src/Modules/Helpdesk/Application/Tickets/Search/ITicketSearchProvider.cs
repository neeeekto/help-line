using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Filters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Sorts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search;


public interface ITicketSearchProvider
{
    protected internal Task<PagedResult<TicketView>> Find(PageData pageData, TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts);
    protected internal Task<IEnumerable<TicketView>> Find(TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts);
    protected internal Task<IEnumerable<string>> FindIds(TicketFilterBase? filter, IEnumerable<TicketSortBase> sorts);
}
