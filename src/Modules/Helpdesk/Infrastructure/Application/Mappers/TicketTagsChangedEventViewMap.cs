using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketTagsChangedEventViewMap : BsonClassMap<TicketTagsChangedEventView>
    {
        public TicketTagsChangedEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketTagsChangedEventView));
            MapMember(x => x.New);
            MapMember(x => x.Old);
        }
    }
}