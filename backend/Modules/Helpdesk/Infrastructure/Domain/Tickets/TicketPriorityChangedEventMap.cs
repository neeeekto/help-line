using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketPriorityChangedEventMap : BsonClassMap<TicketPriorityChangedEvent>
{
    public TicketPriorityChangedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketPriorityChangedEvent));
        MapMember(x => x.Priority).SetSerializer(new EnumSerializer<TicketPriority>(BsonType.String));
        ;
    }
}