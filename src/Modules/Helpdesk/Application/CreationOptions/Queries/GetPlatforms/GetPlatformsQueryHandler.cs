using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Queries.GetPlatforms
{
    internal class GetPlatformsQueryHandler : IQueryHandler<GetPlatformsQuery, IEnumerable<Platform>>
    {
        private readonly IMongoContext _context;

        public GetPlatformsQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Platform>> Handle(GetPlatformsQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _context.GetCollection<Platform>().Find(x => x.ProjectId == request.ProjectId)
                .ToListAsync(cancellationToken: cancellationToken);
            return result;
        }
    }
}
