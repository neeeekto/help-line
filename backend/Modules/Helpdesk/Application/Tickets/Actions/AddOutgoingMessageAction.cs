using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class AddOutgoingMessageAction : TicketActionBase
    {
        public MessageDto Message { get; set; }

        public AddOutgoingMessageAction(MessageDto message)
        {
            Message = message;
        }
    }
}
