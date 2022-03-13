using HelpLine.Modules.Helpdesk.Application.Tickets.Channels;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class ChannelSystemMap : BsonClassMap<ChannelSettings>
    {
        public ChannelSystemMap()
        {
            MapIdProperty("_id");
            SetIsRootClass(true);
        }
    }
}
