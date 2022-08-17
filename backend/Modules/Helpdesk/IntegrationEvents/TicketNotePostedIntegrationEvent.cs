using System;
using HelpLine.Modules.Helpdesk.IntegrationEvents.DTO;

namespace HelpLine.Modules.Helpdesk.IntegrationEvents
{
    public class TicketNotePostedIntegrationEvent : TicketIntegrationEventBase
    {
        public TicketNotePostedIntegrationEvent(Guid id, DateTime occurredOn, string ticketId, InitiatorDto initiator) :
            base(id, occurredOn, ticketId, initiator)
        {
        }
    }
}
