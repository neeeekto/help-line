using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class ScenarioScheduleMap : BsonClassMap<ScenarioSchedule>
    {
        public ScenarioScheduleMap()
        {
            MapIdMember(x => x.Interval);
            AutoMap();
        }
    }
}
