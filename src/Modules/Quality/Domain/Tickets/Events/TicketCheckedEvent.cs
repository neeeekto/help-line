using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Events
{
    public class TicketCheckedEvent : TicketEventBase
    {
        public TicketCheckedEvent(TicketId ticketId, Initiator initiator) : base(ticketId, initiator)
        {
        }
    }
}
