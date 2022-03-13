using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts
{
    public interface ITicketFeedbackDispatcher
    {
        Task Enqueue(TicketId ticketId, TicketFeedbackId feedbackId, IEnumerable<UserChannel> channels, ProjectId projectId);
    }
}
