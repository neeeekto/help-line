using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.BuildingBlocks.Application.Search.Contracts;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Queries.FindTickets
{
    internal class FindTicketsQueryHandler : IQueryHandler<FindTicketsQuery, PagedResult<TicketView>>
    {
        private readonly ISearchProvider<TicketView, TicketFilterCtx> _searchProvider;
        private readonly IFilterContextFactory _filterContextFactory;

        public FindTicketsQueryHandler(ISearchProvider<TicketView, TicketFilterCtx> searchProvider,
            IFilterContextFactory filterContextFactory)
        {
            _searchProvider = searchProvider;
            _filterContextFactory = filterContextFactory;
        }

        public async Task<PagedResult<TicketView>> Handle(FindTicketsQuery request,
            CancellationToken cancellationToken)
        {
            var ctx = await _filterContextFactory.Make();
            return await _searchProvider.Find(request.PageData, request.Filter, ctx, request.Sorts);
        }
    }
}
