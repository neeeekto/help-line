using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class BanSettingsMap : BsonClassMap<BanSettings>
    {
        public BanSettingsMap()
        {
            MapIdMember(x => x.ProjectId);
            AutoMap();
        }
    }
}
