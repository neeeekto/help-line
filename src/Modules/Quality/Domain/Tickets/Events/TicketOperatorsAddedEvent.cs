using System.Collections.Generic;
using HelpLine.Modules.Quality.Domain.Operators;
using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Events
{
    public class TicketOperatorsAddedEvent : TicketEventBase
    {
        public OperatorId OperatorId { get; private set; }

        public TicketOperatorsAddedEvent(TicketId ticketId, Initiator initiator, OperatorId operatorId) : base(ticketId, initiator)
        {
            OperatorId = operatorId;
        }
    }
}
