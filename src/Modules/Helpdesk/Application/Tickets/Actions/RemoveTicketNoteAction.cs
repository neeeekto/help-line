using System;


namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class RemoveTicketNoteAction : TicketActionBase
    {
        public Guid NoteId { get; set; }

        public RemoveTicketNoteAction()
        {
        }

        public RemoveTicketNoteAction(Guid noteId)
        {
            NoteId = noteId;
        }
    }
}
