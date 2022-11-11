using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketMetaViewMap : BsonClassMap<TicketMetaView>
    {
        public TicketMetaViewMap()
        {
            AutoMap();
            MapMember(x => x.Source);
            MapMember(x => x.FromTicket).SetIgnoreIfNull(true);
        }
    }
}