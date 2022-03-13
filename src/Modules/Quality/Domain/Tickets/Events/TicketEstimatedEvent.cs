using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Events
{
    public class TicketEstimatedEvent : TicketEventBase
    {
        public string Key { get; private set; }
        public double Value { get; private set; }
        public bool IsNormal { get; private set; }

        public TicketEstimatedEvent(TicketId ticketId, Initiator initiator, string key, double value, bool isNormal) : base(ticketId, initiator)
        {
            Key = key;
            Value = value;
            IsNormal = isNormal;
        }
    }
}
