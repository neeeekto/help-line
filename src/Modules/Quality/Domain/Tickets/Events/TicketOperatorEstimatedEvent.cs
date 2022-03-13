using System.Collections.ObjectModel;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Quality.Domain.Tickets.State;

namespace HelpLine.Modules.Quality.Domain.Tickets.Events
{
    public class TicketOperatorEstimatedEvent : TicketEventBase
    {
        public OperatorId OperatorId { get; private set; }
        public ReadOnlyDictionary<IndicatorKey, int> Indicators { get; private set; }
        public Message? Comment { get; private set; }

        public TicketOperatorEstimatedEvent(TicketId ticketId, Initiator initiator, OperatorId operatorId,
            ReadOnlyDictionary<IndicatorKey, int> indicators, Message? comment) : base(
            ticketId, initiator)
        {
            OperatorId = operatorId;
            Indicators = indicators;
            Comment = comment;
        }
    }
}
