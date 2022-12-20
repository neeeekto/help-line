using System;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.Modules.Helpdesk.IntegrationEvents.DTO;

namespace HelpLine.Modules.Helpdesk.IntegrationEvents
{
    public class TicketCreatedIntegrationEvent : TicketIntegrationEventBase
    {
        public string ProjectId { get; }

        public TicketCreatedIntegrationEvent(Guid id, DateTime occurredOn, string ticketId, InitiatorDto initiator, string projectId) : base(id, occurredOn, ticketId, initiator)
        {
            ProjectId = projectId;
        }
    }
}
