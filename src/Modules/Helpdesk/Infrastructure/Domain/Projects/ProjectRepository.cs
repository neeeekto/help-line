using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using HelpLine.Modules.Helpdesk.Domain.Projects.Contracts;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Projects
{
    internal class ProjectRepository : EntityRepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(IMongoContext context, IDomainEventCollector eventCollector) : base(context, eventCollector)
        {
        }

        protected override Expression<Func<Project, bool>> GetIdFilter(Project entity) => x => x.Id == entity.Id;

        public Task<Project> Get(ProjectId projectId) => FindOne(x => x.Id == projectId);
    }
}
