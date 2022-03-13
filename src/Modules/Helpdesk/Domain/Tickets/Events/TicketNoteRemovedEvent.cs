using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketNoteRemovedEvent : TicketEventBase
    {
        public TicketNoteId NoteId { get; private set; }

        internal TicketNoteRemovedEvent(TicketId ticketId, Initiator initiator, TicketNoteId noteId) : base(ticketId,
            initiator)
        {
            NoteId = noteId;
        }
    }
}
