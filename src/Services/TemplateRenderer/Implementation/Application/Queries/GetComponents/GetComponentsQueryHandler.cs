using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.TemplateRenderer.Infrastructure;
using HelpLine.Services.TemplateRenderer.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Services.TemplateRenderer.Application.Queries.GetComponents
{
    internal class GetComponentsQueryHandler : IRequestHandler<GetComponentsQuery, IEnumerable<Component>>
    {
        private readonly MongoContext _context;

        public GetComponentsQueryHandler(MongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Component>> Handle(GetComponentsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Components.Find(x => true).ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
