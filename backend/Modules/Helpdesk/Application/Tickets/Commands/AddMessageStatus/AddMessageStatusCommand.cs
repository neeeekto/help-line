using System;
using System.Collections;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.AddMessageStatus
{
    public class AddMessageStatusCommand : CommandBase
    {
        public string TicketId { get; }
        public Guid MessageId { get; }
        public string UserId { get; }
        public MessageStatus Status { get; }
        public string? Reason { get; }
        public IDictionary<string, string>? Meta { get; }

        public AddMessageStatusCommand(string ticketId, Guid messageId, MessageStatus status, string userId, string? reason = null, 
            IDictionary<string, string>? meta = null)
        {
            TicketId = ticketId;
            MessageId = messageId;
            Status = status;
            UserId = userId;
            Meta = meta;
            Reason = reason;
        }
    }
}
