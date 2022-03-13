using System;
using System.Linq;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.ViewModels
{
    public abstract class ChatMessageViewBase
    {
        public MessageView Message { get; internal set; }
        public DateTime Date { get; internal set; }
    }

    public class IncomingChatMessageView : ChatMessageViewBase
    {
    }

    public class OutgoingChatMessageView : ChatMessageViewBase
    {
        public Guid Id { get; internal set; }
        public bool IsUnread { get; internal set; }

        public OutgoingChatMessageView(TicketOutgoingMessageEventView evt, string userId)
        {
            Date = evt.CreateDate;
            Message = evt.Message;
            Id = evt.MessageId;
            IsUnread = evt.Recipients.FirstOrDefault(r => r.UserId == userId)
                ?.DeliveryStatuses.All(x => x.Status != MessageStatus.Viewed) ?? false;
        }
    }
}
