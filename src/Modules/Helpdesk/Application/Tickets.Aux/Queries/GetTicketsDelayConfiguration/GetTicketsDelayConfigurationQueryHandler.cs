using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTicketsDelayConfiguration
{
    internal class
        GetTicketsDelayConfigurationQueryHandler : IQueryHandler<GetTicketsDelayConfigurationQuery,
            TicketsDelayConfiguration>
    {
        private readonly IMongoContext _context;

        public GetTicketsDelayConfigurationQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public Task<TicketsDelayConfiguration> Handle(GetTicketsDelayConfigurationQuery request,
            CancellationToken cancellationToken)
        {
            return _context.GetCollection<TicketsDelayConfiguration>()
                .Find(x => x.ProjectId == request.ProjectId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
