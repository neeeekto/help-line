using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.Helpdesk.Domain.Projects.Contracts
{
    public interface IProjectChecker
    {
        Task<bool> CheckIdUnique(ProjectId id);
    }
}
