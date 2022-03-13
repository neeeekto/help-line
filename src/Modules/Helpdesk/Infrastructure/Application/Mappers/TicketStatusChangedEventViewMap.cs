using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketStatusChangedEventViewMap : BsonClassMap<TicketStatusChangedEventView>
    {
        public TicketStatusChangedEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketStatusChangedEventView));
            MapMember(x => x.New);
            MapMember(x => x.Old);
        }
    }
}