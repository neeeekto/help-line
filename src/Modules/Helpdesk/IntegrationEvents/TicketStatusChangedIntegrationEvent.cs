using System;
using HelpLine.Modules.Helpdesk.IntegrationEvents.DTO;

namespace HelpLine.Modules.Helpdesk.IntegrationEvents
{
    public class TicketStatusChangedIntegrationEvent : TicketIntegrationEventBase
    {
        public TicketStatusDto Status { get; }

        public TicketStatusChangedIntegrationEvent(Guid id, DateTime occurredOn, string ticketId,
            InitiatorDto initiator, TicketStatusDto status) : base(id, occurredOn, ticketId, initiator)
        {
            Status = status;
        }
    }
}
