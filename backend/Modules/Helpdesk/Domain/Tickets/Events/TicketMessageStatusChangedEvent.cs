using System;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketMessageStatusChangedEvent : TicketEventBase
    {
        public TicketOutgoingMessageId MessageId { get; private set; }
        public UserId UserId { get; private set; }
        public MessageStatus Status { get; private set; }
        public string? Detail { get; private set; }

        internal TicketMessageStatusChangedEvent(TicketId ticketId, Initiator initiator,
            TicketOutgoingMessageId messageId, UserId userId, MessageStatus status, string? detail = null) : base(ticketId, initiator)
        {
            MessageId = messageId;
            UserId = userId;
            Status = status;
            Detail = detail;
        }
    }
}
