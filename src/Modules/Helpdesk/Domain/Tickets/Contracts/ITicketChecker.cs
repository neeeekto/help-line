using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts
{
    public interface ITicketChecker
    {
        Task<bool> ProjectIsExist(ProjectId projectId);
        Task<bool> LanguageIsExist(ProjectId projectId, LanguageCode languageCode);
        Task<bool> CheckBan(TicketCreatedEvent evt);

    }
}
