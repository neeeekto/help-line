using HelpLine.Modules.Helpdesk.Application.CreationOptions.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class PlatformMap : BsonClassMap<Platform>
    {
        public PlatformMap()
        {
            MapIdProperty("_id");
            AutoMap();
        }
    }
}
