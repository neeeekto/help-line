using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketIncomingMessageAddedEventMap : BsonClassMap<TicketIncomingMessageAddedEvent>
{
    public TicketIncomingMessageAddedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketIncomingMessageAddedEvent));
        MapMember(x => x.Channel);
        MapMember(x => x.Message);
        MapMember(x => x.UserId);
    }
}