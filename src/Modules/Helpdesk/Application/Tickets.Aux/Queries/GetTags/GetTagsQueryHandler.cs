using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using MongoDB.Driver;
using Tag = HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models.Tag;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTags
{
    internal class GetTagsQueryHandler : IQueryHandler<GetTagsQuery, IEnumerable<Models.Tag>>
    {
        private readonly IMongoContext _context;

        public GetTagsQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            return await _context.GetCollection<Tag>().Find(x => x.ProjectId == request.ProjectId &&
                                                                 (!request.OnlyEnabled || x.Enabled))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
