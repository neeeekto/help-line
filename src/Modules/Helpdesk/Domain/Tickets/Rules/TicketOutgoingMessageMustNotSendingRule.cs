using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketOutgoingMessageMustNotSendingRule : IBusinessRule
    {
        private readonly IEnumerable<TicketOutgoingMessage> _messages;
        private readonly TicketOutgoingMessageId _messageId;
        private readonly UserId _userId;

        internal TicketOutgoingMessageMustNotSendingRule(IEnumerable<TicketOutgoingMessage> messages, TicketOutgoingMessageId messageId, UserId userId)
        {
            _messages = messages;
            _messageId = messageId;
            _userId = userId;
        }

        public string Message => $"The message should not be in status: Sending";
        public bool IsBroken() => _messages.Any(x => x.Id == _messageId && x.Statuses[_userId] == MessageStatus.Sending );
    }
}
