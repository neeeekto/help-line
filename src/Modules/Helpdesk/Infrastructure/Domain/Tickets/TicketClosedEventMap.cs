using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketClosedEventMap : BsonClassMap<TicketClosedEvent>
{
    public TicketClosedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketClosedEvent));
    }
}