using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets
{
    public class Autoreply
    {
        public string Id { get; }
        public Message Message { get; }
        public bool Resolve { get; }
        public IEnumerable<Tag> Tags { get; }
        public TicketReminder? Reminder { get; }

        public Autoreply(string id, Message message, IEnumerable<Tag> tags, TicketReminder? reminder)
        {
            Message = message;
            Resolve = false;
            Tags = tags;
            Reminder = reminder;
            Id = id;
        }

        public Autoreply(string id, Message message, IEnumerable<Tag> tags, bool resolve)
        {
            Message = message;
            Resolve = resolve;
            Id = id;
            Tags = tags;
            Reminder = null;
        }
    }
}
