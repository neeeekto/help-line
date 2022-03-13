using System;
using System.Collections.Generic;
using System.Linq;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.ViewModels
{
    public class TicketChatViewBase
    {
        public string TicketId { get; internal set; }
        public Guid? FeedbackId { get; internal set; }
        public TicketStatusView Status { get; internal set; }

        internal TicketChatViewBase(TicketView ticket)
        {
            var feedbacks = ticket.Events
                .OfType<TicketFeedbackEventView>().OrderByDescending(x => x.CreateDate);

            TicketId = ticket.Id;
            Status = ticket.Status;
            FeedbackId = ticket.Status.Type == TicketStatusType.Resolved
                ? feedbacks.FirstOrDefault()?.FeedbackId
                : null;
        }

        protected IEnumerable<ChatMessageViewBase> GetMessages(TicketView ticket, string userId) => ticket.Events.Where(
                x =>
                    x is TicketIncomingMessageEventView or TicketOutgoingMessageEventView or TicketCreatedEventView)
            .OrderByDescending(x => x.CreateDate)
            .Select(x =>
                {
                    ChatMessageViewBase result = x switch
                    {
                        TicketIncomingMessageEventView im => new IncomingChatMessageView()
                        {
                            Date = im.CreateDate,
                            Message = im.Message
                        },
                        TicketCreatedEventView ct => new IncomingChatMessageView()
                        {
                            Date = ct.CreateDate,
                            Message = ct.Message ?? new MessageView() {Text = ""}
                        },
                        TicketOutgoingMessageEventView om => new OutgoingChatMessageView(om, userId)
                    };
                    return result;
                }
            );
    }
}
