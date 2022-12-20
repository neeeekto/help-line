using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.RetryOutgoingMessage
{
    public class RetryOutgoingMessageCommand : CommandBase
    {
        public string TicketId { get; }
        public Guid MessageId { get; }
        public string UserId { get; }
        public InitiatorDto Initiator { get; }

        public RetryOutgoingMessageCommand(string ticketId, Guid messageId, string userId, InitiatorDto initiator)
        {
            TicketId = ticketId;
            MessageId = messageId;
            UserId = userId;
            Initiator = initiator;
        }
    }
}
