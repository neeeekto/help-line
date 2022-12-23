using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketMessageStatusChangedEventMap : BsonClassMap<TicketMessageStatusChangedEvent>
{
    public TicketMessageStatusChangedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketMessageStatusChangedEvent));
        MapMember(x => x.Detail).SetIgnoreIfNull(true);
        MapMember(x => x.Meta).SetIgnoreIfNull(true);
        MapMember(x => x.Status);
        MapMember(x => x.MessageId);
        MapMember(x => x.UserId);
    }
}