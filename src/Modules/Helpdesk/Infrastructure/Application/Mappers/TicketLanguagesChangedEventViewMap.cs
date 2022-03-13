using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketLanguagesChangedEventViewMap : BsonClassMap<TicketLanguagesChangedEventView>
    {
        public TicketLanguagesChangedEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketLanguagesChangedEventView));
            MapMember(x => x.From);
            MapMember(x => x.To);
        }
    }
}