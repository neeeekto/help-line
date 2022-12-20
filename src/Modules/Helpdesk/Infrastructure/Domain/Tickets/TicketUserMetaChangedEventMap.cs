using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketUserMetaChangedEventMap : BsonClassMap<TicketUserMetaChangedEvent>
{
    public TicketUserMetaChangedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketUserMetaChangedEvent));
        MapMember(x => x.Meta);

    }
}