using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Queries.GetAutoreplyScenarios
{
    internal class
        GetAutoreplyScenariosQueryHandler : IQueryHandler<GetAutoreplyScenariosQuery, IEnumerable<AutoreplyScenario>>
    {
        private readonly IMongoContext _context;

        public GetAutoreplyScenariosQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AutoreplyScenario>> Handle(GetAutoreplyScenariosQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _context.GetCollection<AutoreplyScenario>()
                .Find(x => x.ProjectId == request.ProjectId)
                .ToListAsync(cancellationToken: cancellationToken);
            return result;
        }
    }
}
