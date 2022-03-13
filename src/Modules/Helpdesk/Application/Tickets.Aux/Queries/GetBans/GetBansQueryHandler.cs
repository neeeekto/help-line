using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetBans
{
    internal class GetBansQueryHandler : IQueryHandler<GetBansQuery, IEnumerable<Ban>>
    {
        private readonly IMongoContext _ctx;

        public GetBansQueryHandler(IMongoContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<Ban>> Handle(GetBansQuery request,
            CancellationToken cancellationToken)
        {
            return await _ctx.GetCollection<Ban>().Find(x => x.ProjectId == request.ProjectId)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
