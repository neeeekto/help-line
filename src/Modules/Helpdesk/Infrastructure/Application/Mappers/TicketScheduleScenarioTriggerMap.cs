using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketScheduleScenarioTriggerMap : BsonClassMap<ScheduleScenarioTrigger>
    {
        public TicketScheduleScenarioTriggerMap()
        {
            SetDiscriminator(nameof(ScheduleScenarioTrigger));
            AutoMap();
        }
    }
}