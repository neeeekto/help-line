using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketStatusChangedScenarioTriggerMap : BsonClassMap<TicketStatusChangedScenarioTrigger>
    {
        public TicketStatusChangedScenarioTriggerMap()
        {
            MapMember(x => x.From);
            SetDiscriminator(nameof(TicketStatusChangedScenarioTrigger));
        }
    }
}