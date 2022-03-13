using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketOutgoingMessageAddedEvent : TicketEventBase
    {
        public TicketOutgoingMessageId MessageId { get; private set; }
        public Message Message { get; private set; }

        public UserChannels Recipients { get; private set; }

        internal TicketOutgoingMessageAddedEvent(TicketId ticketId, Initiator initiator, Message message,
            UserChannels recipients, TicketOutgoingMessageId messageId) : base(ticketId, initiator)
        {
            Message = message;
            Recipients = recipients;
            MessageId = messageId;
        }
    }
}
