using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Queries.GetTicketChatDiscussion
{
    public class GetTicketChatDiscussionQuery : QueryBase<TicketChatDiscussionView>
    {
        public string UserId { get; }
        public string TicketId { get; }

        public GetTicketChatDiscussionQuery(string userId, string ticketId)
        {
            UserId = userId;
            TicketId = ticketId;
        }


    }
}
