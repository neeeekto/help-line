using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketUserUnsubscribedEventViewMap : BsonClassMap<TicketUserUnsubscribedEventView>
    {
        public TicketUserUnsubscribedEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketUserUnsubscribedEventView));
        }
    }
}