using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Search.Queries.FindTickets
{
    internal class FindTicketsQueryHandler : IQueryHandler<FindTicketsQuery, PagedResult<TicketView>>
    {
        private readonly ITicketSearchProvider _searchProvider;
        private readonly IFilterContextFactory _filterContextFactory;

        public FindTicketsQueryHandler(ITicketSearchProvider searchProvider,
            IFilterContextFactory filterContextFactory)
        {
            _searchProvider = searchProvider;
            _filterContextFactory = filterContextFactory;
        }

        public async Task<PagedResult<TicketView>> Handle(FindTicketsQuery request,
            CancellationToken cancellationToken)
        {
            return await _searchProvider.Find(request.PageData, request.Filter, request.Sorts);
        }
    }
}
