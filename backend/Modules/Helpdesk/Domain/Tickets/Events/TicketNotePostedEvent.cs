using System.Collections.Generic;
using System.Linq;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketNotePostedEvent : TicketEventBase
    {
        public TicketNoteId NoteId { get; private set; }
        public Message Message { get; private set; }
        public IEnumerable<string> Tags { get; private set; } // Это сборная параша для pinned, private и прочего - домен этим не рулит - на откуп приложухи

        internal TicketNotePostedEvent(TicketId ticketId, Initiator initiator, TicketNoteId noteId,  Message message, IEnumerable<string> tags) : base(ticketId, initiator)
        {
            Message = message;
            Tags = tags.Distinct();
            NoteId = noteId;
        }
    }
}
