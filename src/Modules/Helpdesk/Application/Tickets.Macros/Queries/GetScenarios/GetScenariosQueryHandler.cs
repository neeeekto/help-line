using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Queries.GetScenarios
{
    class GetScenariosQueryHandler : IQueryHandler<GetScenariosQuery, IEnumerable<Scenario>>
    {
        private readonly IMongoContext _context;

        public GetScenariosQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Scenario>> Handle(GetScenariosQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.GetCollection<Scenario>().Find(x => true)
                .ToListAsync(cancellationToken);
        }
    }
}