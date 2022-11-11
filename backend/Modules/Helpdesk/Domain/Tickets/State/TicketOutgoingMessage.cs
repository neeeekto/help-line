using System.Collections.Generic;
using System.Linq;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class TicketOutgoingMessage
    {
        public TicketOutgoingMessageId Id { get; }
        private Dictionary<UserId, MessageStatus> _statuses;
        public IReadOnlyDictionary<UserId, MessageStatus> Statuses => _statuses;

        public Message Message { get; }
        public Initiator Initiator { get; }

        public TicketOutgoingMessage(TicketOutgoingMessageId id, Message message, Initiator initiator, UserChannels channels)
        {
            Id = id;
            _statuses = channels.ToDictionary(x => x.UserId, _ => MessageStatus.Sending);
            Message = message;
            Initiator = initiator;
        }

        internal void ChangeStatus(UserId userId, MessageStatus status)
        {
            _statuses[userId] = status;
        }
    }
}
