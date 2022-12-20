using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events
{
    public class TicketNoteEventView : TicketEventView
    {
        public Guid NoteId { get; internal set; }
        public MessageView Message { get; internal set; }
        public IEnumerable<string> Tags { get; internal set; }
        public DateTime? DeleteTime { get; internal set; }
        public IEnumerable<HistoryRecord> History { get; internal set; }

        public class HistoryRecord
        {
            public InitiatorView Initiator { get; internal set; }
            public MessageView Message { get; internal set; }
            public DateTime Date { get; internal set; }
        }
    }
}
