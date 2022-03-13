using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketUserIdsChangedEventViewMap : BsonClassMap<TicketUserIdsChangedEventView>
    {
        public TicketUserIdsChangedEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketUserIdsChangedEventView));
            MapMember(x => x.New);
            MapMember(x => x.Old);
        }
    }
}