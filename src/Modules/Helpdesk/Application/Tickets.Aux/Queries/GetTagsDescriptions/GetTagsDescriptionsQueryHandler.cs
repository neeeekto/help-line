using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Queries.GetTagsDescriptions
{
    internal class
        GetTagsDescriptionsQueryHandler : IQueryHandler<GetTagsDescriptionsQuery, IEnumerable<TagsDescription>>
    {
        private readonly IMongoContext _context;

        public GetTagsDescriptionsQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TagsDescription>> Handle(GetTagsDescriptionsQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _context.GetCollection<TagsDescription>().Find(x =>
                    x.Enabled &&
                    x.ProjectId == request.ProjectId &&
                    (!request.Tags.Any() || request.Tags.Contains(x.Tag)))
                .ToListAsync(cancellationToken);
            if (request.Audience.Any())
            {
                foreach (var tagsDescription in result)
                    tagsDescription.Issues =
                        tagsDescription.Issues.Where(x => x.Audience.Any(x => request.Audience!.Contains(x)));

                result = result.Where(x => x.Issues.Any()).ToList();
            }

            return result;
        }
    }
}
