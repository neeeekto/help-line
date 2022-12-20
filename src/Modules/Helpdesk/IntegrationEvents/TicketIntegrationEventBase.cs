using System;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.Modules.Helpdesk.IntegrationEvents.DTO;

namespace HelpLine.Modules.Helpdesk.IntegrationEvents
{
    public class TicketIntegrationEventBase: IntegrationEvent
    {
        public string TicketId { get; }
        public InitiatorDto Initiator { get; }

        public TicketIntegrationEventBase(Guid id, DateTime occurredOn, string ticketId, InitiatorDto initiator) : base(id, occurredOn)
        {
            TicketId = ticketId;
            Initiator = initiator;
        }
    }
}
