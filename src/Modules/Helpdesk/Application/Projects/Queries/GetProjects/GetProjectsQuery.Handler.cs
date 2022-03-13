using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Queries;
using HelpLine.Modules.Helpdesk.Application.Projects.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Projects.Queries.GetProjects
{
    internal class GetProjectsQueryHandler : IQueryHandler<GetProjectsQuery, IEnumerable<ProjectView>>
    {
        private readonly IMongoContext _context;
        private readonly IMapper _mapper;

        public GetProjectsQueryHandler(IMongoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectView>> Handle(GetProjectsQuery request,
            CancellationToken cancellationToken)
        {
            var collection = _context.GetCollection<Project>();
            var items = await collection.Find(x => true).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ProjectView>>(items);
        }
    }
}
