using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts
{
    public interface IUnsubscribeManager
    {
        Task TryRemove(UserId userId, ProjectId projectId);
        Task Add(UserId userId, ProjectId projectId, string message);
        Task<bool> CheckExist(UserId userId, ProjectId projectId);
    }
}
