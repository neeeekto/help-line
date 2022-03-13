using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Rules
{
    public class TicketOutgoingMessageMustExistRule : IBusinessRule
    {
        private readonly IEnumerable<TicketOutgoingMessage> _messages;
        private readonly TicketOutgoingMessageId _messageId;

        internal TicketOutgoingMessageMustExistRule(IEnumerable<TicketOutgoingMessage> messages, TicketOutgoingMessageId messageId)
        {
            _messages = messages;
            _messageId = messageId;
        }

        public string Message => $"Message must be exist";
        public bool IsBroken() => _messages.All(x => x.Id != _messageId);
    }
}
