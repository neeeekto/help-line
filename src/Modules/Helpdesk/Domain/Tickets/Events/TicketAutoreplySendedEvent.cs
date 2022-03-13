using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketAutoreplySendedEvent : TicketEventBase
    {
        public Message Message { get; private set; }
        public bool Resolve { get; private set; }
        public IEnumerable<Tag> Tags { get; private set; }
        public TicketReminder? Reminder { get; private set; }

        internal TicketAutoreplySendedEvent(TicketId ticketId, Initiator initiator, Message message, bool resolve, IEnumerable<Tag> tags, TicketReminder? reminder) : base(ticketId, initiator)
        {
            Message = message;
            Resolve = resolve;
            Tags = tags;
            Reminder = reminder;
        }
    }
}
