using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TagMap : BsonClassMap<Tag>
    {
        public TagMap()
        {
            AutoMap();
            SetIgnoreExtraElements(true);
        }
    }
}
