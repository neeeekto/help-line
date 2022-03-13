using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Queries.GetTicketsForChat
{
    public class GetTicketsForChatQuery : QueryBase<IEnumerable<TicketForChatView>>
    {
        public string UserId { get; }
        public string ProjectId { get; }

        public GetTicketsForChatQuery(string userId, string projectId)
        {
            UserId = userId;
            ProjectId = projectId;
        }
    }
}
