using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Commands.AddIncomingMessage
{
    public class AddIncomingMessageCommand : CommandBase
    {
        public string TicketId { get; }
        public MessageDto Message { get; }
        public string UserId { get; }
        public string Channel { get; }

        public AddIncomingMessageCommand(string ticketId, MessageDto message, string userId, string channel)
        {
            TicketId = ticketId;
            Message = message;
            UserId = userId;
            Channel = channel;
        }


    }
}
