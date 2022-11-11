using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Events
{
    public class TicketCreatedEvent : TicketEventBase
    {
        public TicketCreatedEvent(TicketId ticketId, Initiator initiator) : base(ticketId, initiator)
        {
        }
    }
}
