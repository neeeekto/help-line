using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.CreationOptions.Queries.GetProblemAndTheme
{
    internal class
        GetProblemAndThemeQueryHandler : IQueryHandler<GetProblemAndThemeQuery, IEnumerable<ProblemAndThemeRoot>>
    {
        private readonly IMongoContext _context;

        public GetProblemAndThemeQueryHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProblemAndThemeRoot>> Handle(GetProblemAndThemeQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _context.GetCollection<ProblemAndThemeRoot>().Find(x =>
                    x.ProjectId == request.ProjectId &&
                    (!request.OnlyEnabled || x.Enabled))
                .ToListAsync(cancellationToken: cancellationToken);
            if (request.OnlyEnabled)
                result.ForEach(FilterEnabled);

            return result;
        }

        private void FilterEnabled(ProblemAndTheme entity)
        {
            if (entity.Subtypes != null)
            {
                entity.Subtypes = entity.Subtypes.Where(x => x.Enabled);
                foreach (var problemAndTheme in entity.Subtypes)
                    FilterEnabled(problemAndTheme);
            }
        }
    }
}
