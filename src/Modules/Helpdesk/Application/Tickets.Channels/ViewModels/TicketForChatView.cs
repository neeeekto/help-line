using System;
using System.Linq;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.ViewModels
{
    public class TicketForChatView : TicketChatViewBase
    {
        public ChatMessageViewBase LastMessage { get; internal set; }
        public bool IsClosed { get; internal set; }
        public bool IsUnread { get; internal set; }

        internal TicketForChatView(TicketView ticket, string userId) : base(ticket)
        {
            IsClosed = ticket.Status.Kind == TicketStatusKind.Closed;
            IsUnread = ticket.Events.OfType<TicketOutgoingMessageEventView>().Any(x => x.Recipients
                .Any(x => x.Channel == Models.Channels.Chat &&
                          !x.DeliveryStatuses.All(x => x.Status == MessageStatus.Viewed)));
            LastMessage = GetMessages(ticket, userId).First();
        }
    }
}
