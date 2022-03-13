using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketEventViewMap : BsonClassMap<TicketEventView>
    {
        public TicketEventViewMap()
        {
            MapIdMember(x => x.Id);
            AutoMap();
            MapMember(x => x.Initiator);
            MapMember(x => x.CreateDate);
        }
    }
}
