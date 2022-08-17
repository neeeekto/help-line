using HelpLine.Modules.Helpdesk.Application.Tickets.Automations;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class AutoreplyScenarioActionMap : BsonClassMap<AutoreplyScenarioAction>
    {
        public AutoreplyScenarioActionMap()
        {
            AutoMap();
        }
    }
}
