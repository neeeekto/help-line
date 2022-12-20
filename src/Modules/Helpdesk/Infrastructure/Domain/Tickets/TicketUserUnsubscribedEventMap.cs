using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketUserUnsubscribedEventMap : BsonClassMap<TicketUserUnsubscribedEvent>
{
    public TicketUserUnsubscribedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketUserUnsubscribedEvent));
    }
}