using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.Modules.Quality.Domain.Tickets.Events;

namespace HelpLine.Modules.Quality.Domain.Tickets.State
{
    public class TicketState : IEventsSourcingAggregateState
    {
        internal TicketState()
        {
        }

        private static void Apply(EventBase<TicketId> evt) // For ignore
        {
        }

        internal void ApplyEvent(EventBase<TicketId> evt)
        {
            Apply((dynamic) evt);
        }

        public TicketStatus Status { get; private set; }
        public Reason Reason { get; private set; }

        private void Apply(TicketCreatedEvent evt)
        {
            Status = TicketStatus.Created;
        }

        private void Apply(TicketCheckedEvent evt)
        {
            Status = TicketStatus.Checked;
        }

        private void Apply(TicketSendToReviewEvent evt)
        {
            Status = TicketStatus.Checking;
        }
    }
}
