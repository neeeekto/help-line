using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Events
{
    public class TicketComplaintAddedEvent : TicketEventBase
    {
        public OperatorId Who { get; private set; }
        public Message Reason { get; private set; }

        public TicketComplaintAddedEvent(TicketId ticketId, Initiator initiator, OperatorId who, Message reason) : base(ticketId, initiator)
        {
            Who = who;
            Reason = reason;
        }
    }
}
