using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using HelpLine.Modules.Helpdesk.Domain.Projects.Contracts;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Projects
{
    internal class ProjectChecker : IProjectChecker
    {
        private readonly IMongoContext _context;

        public ProjectChecker(IMongoContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIdUnique(ProjectId projectId)
        {
            var collection = _context.GetCollection<Project>();
            var count = await collection.CountDocumentsAsync(x => x.Id == projectId);
            return count == 0;
        }
    }
}
