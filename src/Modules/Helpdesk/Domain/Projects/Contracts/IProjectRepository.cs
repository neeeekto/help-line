using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.Helpdesk.Domain.Projects.Contracts
{
    public interface IProjectRepository
    {
        Task<Project> Get(ProjectId projectId);
        Task Add(Project project);
        Task Update(Project project);

    }
}
