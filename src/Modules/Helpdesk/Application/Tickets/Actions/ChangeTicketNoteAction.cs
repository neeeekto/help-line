using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;


namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class ChangeTicketNoteAction : TicketActionBase
    {
        public Guid NoteId { get; set; }
        public MessageDto Message { get; set; }
        public IEnumerable<string>? Tags { get; set; }

        public ChangeTicketNoteAction()
        {
        }

        public ChangeTicketNoteAction(Guid noteId, MessageDto message)
        {
            NoteId = noteId;
            Message = message;
        }

        public ChangeTicketNoteAction(Guid noteId, MessageDto message, IEnumerable<string> tags)
        {
            NoteId = noteId;
            Message = message;
            Tags = tags;
        }
    }
}
