using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketTagsChangedEvent : TicketEventBase
    {
        public IEnumerable<Tag> Tags { get; private set; }
        public TicketTagsChangedEvent(TicketId ticketId, Initiator initiator, IEnumerable<Tag> tags) : base(ticketId, initiator)
        {
            Tags = tags;
        }
    }
}
