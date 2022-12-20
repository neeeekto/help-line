using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketUserIdsChangedEventMap : BsonClassMap<TicketUserIdsChangedEvent>
{
    public TicketUserIdsChangedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketUserIdsChangedEvent));
        MapMember(x => x.UserIds);

    }
}