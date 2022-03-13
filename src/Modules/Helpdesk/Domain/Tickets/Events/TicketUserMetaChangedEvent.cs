using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketUserMetaChangedEvent : TicketEventBase
    {
        public UserMeta Meta { get; private set; }

        internal TicketUserMetaChangedEvent(TicketId ticketId, Initiator initiator, UserMeta meta) : base(ticketId, initiator)
        {
            Meta = meta;
        }
    }
}
