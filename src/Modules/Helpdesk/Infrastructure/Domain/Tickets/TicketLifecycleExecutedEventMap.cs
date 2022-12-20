using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketLifecycleExecutedEventMap : BsonClassMap<TicketLifecycleExecutedEvent>
{
    public TicketLifecycleExecutedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketLifecycleExecutedEvent));
        MapMember(x => x.LifeCycleType).SetSerializer(new EnumSerializer<TicketLifeCycleType>(BsonType.String));

    }
}