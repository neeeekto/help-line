using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketTagsChangedEventMap : BsonClassMap<TicketTagsChangedEvent>
{
    public TicketTagsChangedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketTagsChangedEvent));
        MapMember(x => x.Tags);
    }
}