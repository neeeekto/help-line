using System;
using HelpLine.Modules.Helpdesk.IntegrationEvents.DTO;

namespace HelpLine.Modules.Helpdesk.IntegrationEvents
{
    public class TicketFeedbackAddedIntegrationEvent : TicketIntegrationEventBase
    {
        public int Score { get; }
        public bool? Solved { get; }

        public TicketFeedbackAddedIntegrationEvent(Guid id, DateTime occurredOn, string ticketId,
            InitiatorDto initiator, int score, bool? solved) : base(id, occurredOn, ticketId, initiator)
        {
            Score = score;
            Solved = solved;
        }
    }
}
