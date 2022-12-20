using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts
{
    public interface ITicketConfigurations
    {
        Task<TimeSpan> GetLifeCycleDelay(ProjectId projectId, TicketLifeCycleType type); // получить задержку для жизненного цикла
        Task<TimeSpan> GetInactivityDelay(ProjectId projectId);
        Task<TimeSpan> GetFeedbackCompleteDelay(ProjectId scope);
    }
}
