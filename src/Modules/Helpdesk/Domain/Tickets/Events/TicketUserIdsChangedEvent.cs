using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Events
{
    public class TicketUserIdsChangedEvent : TicketEventBase
    {
        public IEnumerable<UserIdInfo> UserIds { get; private set; }

        internal TicketUserIdsChangedEvent(TicketId ticketId, Initiator initiator, IEnumerable<UserIdInfo> userIds) : base(ticketId, initiator)
        {
            UserIds = userIds;
        }
    }
}
