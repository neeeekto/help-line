using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.TemplateRenderer.Infrastructure;
using HelpLine.Services.TemplateRenderer.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Services.TemplateRenderer.Application.Queries.GetContexts
{
    internal class GetContextsQueryHandler : IRequestHandler<GetContextsQuery, IEnumerable<Context>>
    {
        private readonly MongoContext _context;

        public GetContextsQueryHandler(MongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Context>> Handle(GetContextsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Contexts.Find(x => true).ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
