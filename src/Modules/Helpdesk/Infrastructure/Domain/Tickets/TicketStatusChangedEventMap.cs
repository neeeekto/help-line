using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketStatusChangedEventMap : BsonClassMap<TicketStatusChangedEvent>
{
    public TicketStatusChangedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketStatusChangedEvent));
        MapMember(x => x.Status);
    }
}