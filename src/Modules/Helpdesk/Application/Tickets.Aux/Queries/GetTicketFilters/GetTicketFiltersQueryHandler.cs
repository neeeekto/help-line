using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketFilters
{
    internal class GetTicketFiltersQueryHandler : IQueryHandler<GetTicketFiltersQuery, IEnumerable<TicketFilter>>,
        IQueryHandler<GetTicketFilterQuery, TicketFilter?>
    {
        private readonly IMongoContext _context;
        private readonly IExecutionContextAccessor _accessor;


        public GetTicketFiltersQueryHandler(IMongoContext context, IExecutionContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task<IEnumerable<TicketFilter>> Handle(GetTicketFiltersQuery request,
            CancellationToken cancellationToken)
        {
            var collection = _context.GetCollection<TicketFilter>();
            var fb = new FilterDefinitionBuilder<TicketFilter>();
            var filter = fb.Eq(x => x.ProjectId, request.ProjectId);
            if (request.ForOperatorId != null)
            {
                filter &= fb.Where(x => x.Share is TicketFilterShareGlobal)
                          | fb.Eq(x => x.Owner, request.ForOperatorId)
                          | (fb.Where(x => x.Share is TicketFilterShareForOperators)
                             & fb.OfType<TicketFilterShareBase, TicketFilterShareForOperators>(x => x.Share,
                                 x => x.Operators.Any(x => x == request.ForOperatorId)));
            }

            if (request.Features?.Any() == true)
                filter &= fb.Where(x => x.Features.Any(f => request.Features.Contains(f)));

            var result = await collection.Find(filter).SortBy(x => x.Order).ToListAsync(cancellationToken: cancellationToken);
            return result;
        }

        public async Task<TicketFilter> Handle(GetTicketFilterQuery request, CancellationToken cancellationToken)
        {
            var filter = await _context.GetCollection<TicketFilter>().Find(x => x.Id == request.FilterId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken)!;
            if (filter == null)
                throw new NotFoundException(request.FilterId);
            return filter;
        }
    }
}
