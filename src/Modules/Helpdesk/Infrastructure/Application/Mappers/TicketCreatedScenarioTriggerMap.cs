using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketCreatedScenarioTriggerMap : BsonClassMap<TicketCreatedScenarioTrigger>
    {
        public TicketCreatedScenarioTriggerMap()
        {
            SetDiscriminator(nameof(TicketCreatedScenarioTrigger));
            AutoMap();
        }
    }
}