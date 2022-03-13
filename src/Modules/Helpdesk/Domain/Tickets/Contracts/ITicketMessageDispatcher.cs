using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts
{
    public interface ITicketMessageDispatcher
    {
        Task Enqueue(TicketId ticketId, TicketOutgoingMessageId messageId, Message message, IEnumerable<UserChannel> channels, ProjectId projectId);
    }
}
