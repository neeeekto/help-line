using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetUnsubscribes
{
    internal class GetUnsubscribesQueryHandler : IQueryHandler<GetUnsubscribesQuery, IEnumerable<Unsubscribe>>
    {
        private readonly IMongoContext _context;

        public GetUnsubscribesQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Unsubscribe>> Handle(GetUnsubscribesQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.GetCollection<Unsubscribe>().Find(x => x.ProjectId == request.ProjectId)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
