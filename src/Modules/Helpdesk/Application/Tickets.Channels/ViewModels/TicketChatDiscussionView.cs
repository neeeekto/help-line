using System.Collections.Generic;
using System.Linq;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.ViewModels
{
    public class TicketChatDiscussionView : TicketChatViewBase
    {
        public IEnumerable<ChatMessageViewBase> Messages { get; set; }
        public IEnumerable<FeedbackReviewView> Feedbacks { get; internal set; }

        internal TicketChatDiscussionView(TicketView ticket, string userId) : base(ticket)
        {
            Feedbacks = ticket.Feedbacks.OrderByDescending(x => x.DateTime);
            Messages = GetMessages(ticket, userId);
        }
    }
}
