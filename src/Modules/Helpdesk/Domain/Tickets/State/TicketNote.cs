using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.State
{
    public class TicketNote : ValueObject
    {
        public TicketNoteId NoteId { get; }
        public Message Message { get; }
        public IEnumerable<string> Tags { get; }

        public TicketNote(TicketNoteId noteId, Message message,IEnumerable<string> tags)
        {
            NoteId = noteId;
            Tags = tags;
            Message = message;
        }
    }
}
