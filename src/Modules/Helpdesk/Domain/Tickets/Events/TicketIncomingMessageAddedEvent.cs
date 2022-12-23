using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketIncomingMessageAddedEvent : TicketEventBase
    {
        public Message Message { get; private set; }
        public UserId UserId { get; private set; }
        public Channel Channel { get; private set; }

        internal TicketIncomingMessageAddedEvent(TicketId ticketId, Message message, UserId userId,
            Channel channel) : base(ticketId, new UserInitiator(userId))
        {
            Message = message;
            UserId = userId;
            Channel = channel;
        }
    }
}
