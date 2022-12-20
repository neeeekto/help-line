using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketUserMetaChangedEventViewMap : BsonClassMap<TicketUserMetaChangedEventView>
    {
        public TicketUserMetaChangedEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketUserMetaChangedEventView));
            MapMember(x => x.New);
            MapMember(x => x.Old);
        }
    }
}