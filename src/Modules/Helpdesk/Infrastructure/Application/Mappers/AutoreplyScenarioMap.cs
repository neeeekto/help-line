using HelpLine.Modules.Helpdesk.Application.Tickets.Automations;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class AutoreplyScenarioMap : BsonClassMap<AutoreplyScenario>
    {
        public AutoreplyScenarioMap()
        {
            MapIdMember(x => x.Id);
            AutoMap();
        }
    }
}
