using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class AddTicketNoteAction : TicketActionBase
    {
        public MessageDto Message { get; set; }
        public IEnumerable<string>? Tags { get; set; }

        public AddTicketNoteAction()
        {
        }

        public AddTicketNoteAction(MessageDto message)
        {
            Message = message;
        }

        public AddTicketNoteAction(MessageDto message, IEnumerable<string>? tags)
        {
            Message = message;
            Tags = tags;
        }
    }
}
