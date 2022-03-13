using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Events
{
    public class TicketIndicatorSavedEvent : TicketEventBase
    {
        public Indicator Indicator { get; private set; }

        public TicketIndicatorSavedEvent(TicketId ticketId, Initiator initiator, Indicator indicator) :
            base(ticketId, initiator)
        {
            Indicator = indicator;
        }
    }
}
