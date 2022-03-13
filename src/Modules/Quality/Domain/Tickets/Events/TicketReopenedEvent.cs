using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Events
{
    public class TicketReopenedEvent : TicketEventBase
    {
        public TicketReopenedEvent(TicketId ticketId, Initiator initiator) : base(ticketId, initiator)
        {
        }
    }
}
