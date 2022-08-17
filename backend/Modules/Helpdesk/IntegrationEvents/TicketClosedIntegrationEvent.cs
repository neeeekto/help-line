using System;
using HelpLine.Modules.Helpdesk.IntegrationEvents.DTO;

namespace HelpLine.Modules.Helpdesk.IntegrationEvents
{
    public class TicketClosedIntegrationEvent : TicketIntegrationEventBase
    {
        public TicketClosedIntegrationEvent(Guid id, DateTime occurredOn, string ticketId, InitiatorDto initiator) :
            base(id, occurredOn, ticketId, initiator)
        {
        }
    }
}
