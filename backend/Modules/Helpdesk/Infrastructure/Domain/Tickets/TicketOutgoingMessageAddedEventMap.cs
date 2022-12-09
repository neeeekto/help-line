using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketOutgoingMessageAddedEventMap : BsonClassMap<TicketOutgoingMessageAddedEvent>
{
    public TicketOutgoingMessageAddedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketOutgoingMessageAddedEvent));
        MapMember(x => x.Recipients);
        MapMember(x => x.Message);
        MapMember(x => x.Recipients);
        MapMember(x => x.MessageId);
    }
}