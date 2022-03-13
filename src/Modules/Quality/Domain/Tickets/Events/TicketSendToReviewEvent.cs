using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Events
{
    public class TicketSendToReviewEvent : TicketEventBase
    {
        public Reason Reason { get; private set; }

        public TicketSendToReviewEvent(TicketId ticketId, Initiator initiator, Reason reason) : base(ticketId, initiator)
        {
            Reason = reason;
        }
    }
}
