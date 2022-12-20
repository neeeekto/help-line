using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketClosedEventViewMap : BsonClassMap<TicketClosedEventView>
    {
        public TicketClosedEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketClosedEventView));
        }
    }
}