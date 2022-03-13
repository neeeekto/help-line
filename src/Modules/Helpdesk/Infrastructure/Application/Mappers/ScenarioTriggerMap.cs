using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class ScenarioTriggerMap : BsonClassMap<ScenarioTriggerBase>
    {
        public ScenarioTriggerMap()
        {
            SetIsRootClass(true);
            MapMember(x => x.Event);
        }
    }
}