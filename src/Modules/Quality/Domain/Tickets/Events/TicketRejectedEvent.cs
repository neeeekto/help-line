using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Events
{
    public class TicketRejectedEvent  : TicketEventBase
    {
        public TicketRejectedEvent(TicketId ticketId, Initiator initiator) : base(ticketId, initiator)
        {
        }
    }
}
