using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts
{
    public interface ITicketIdFactory
    {
        Task<TicketId> GetNext(ProjectId projectId);
    }
}
