using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Events
{
    public class TicketOperatorMarkedForEvaluationEvent : TicketEventBase
    {
        public OperatorId OperatorId { get; private set; }

        public TicketOperatorMarkedForEvaluationEvent(TicketId ticketId, Initiator initiator, OperatorId operatorId) :
            base(ticketId, initiator)
        {
            OperatorId = operatorId;
        }
    }
}
