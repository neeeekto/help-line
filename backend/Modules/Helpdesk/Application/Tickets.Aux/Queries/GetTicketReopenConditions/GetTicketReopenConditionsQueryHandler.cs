using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketReopenConditions
{
    internal class GetTicketReopenConditionsQueryHandler : IQueryHandler<GetTicketReopenConditionsQuery, IEnumerable<TicketReopenCondition>>
    {
        private readonly IMongoContext _context;

        public GetTicketReopenConditionsQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketReopenCondition>> Handle(GetTicketReopenConditionsQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _context.GetCollection<TicketReopenCondition>()
                .Find(x => x.ProjectId == request.ProjectId).ToListAsync(cancellationToken: cancellationToken);
            return result;
        }
    }
}
