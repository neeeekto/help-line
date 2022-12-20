using HelpLine.Modules.Helpdesk.Application.Tickets.Macros;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Core;
using HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class ScenarioMap : BsonClassMap<Scenario>
    {
        public ScenarioMap()
        {
            MapIdMember(x => x.Id);
            AutoMap();
        }
    }
}
