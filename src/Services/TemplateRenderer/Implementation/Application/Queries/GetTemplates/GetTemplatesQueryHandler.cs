using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.Services.TemplateRenderer.Infrastructure;
using HelpLine.Services.TemplateRenderer.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Services.TemplateRenderer.Application.Queries.GetTemplates
{
    internal class GetTemplatesQueryHandler : IRequestHandler<GetTemplatesQuery, IEnumerable<Template>>
    {
        private readonly MongoContext _context;

        public GetTemplatesQueryHandler(MongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Template>> Handle(GetTemplatesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Templates.Find(x => true).ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
