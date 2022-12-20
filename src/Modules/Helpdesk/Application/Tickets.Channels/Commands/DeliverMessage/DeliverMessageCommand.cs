using System;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using Newtonsoft.Json;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.DeliverMessage
{
    internal class DeliverMessageCommand : InternalCommandBase
    {
        public string TicketId { get; }
        public Guid MessageId { get; }
        public string ProjectId { get; }
        public MessageDto Message { get; }
        public string Channel { get; }
        public string UserId { get; }

        [JsonConstructor]
        public DeliverMessageCommand(Guid id, string ticketId, Guid messageId, string projectId, MessageDto message,
            string channel, string userId) : base(id)
        {
            TicketId = ticketId;
            MessageId = messageId;
            ProjectId = projectId;
            Message = message;
            Channel = channel;
            UserId = userId;
        }
    }
}
