using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketTimerPlannedEventMap : BsonClassMap<TicketLifecyclePlannedEvent>
{
    public TicketTimerPlannedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketLifecyclePlannedEvent));
        MapMember(x => x.ExecutionDate);
        MapMember(x => x.LifeCycleType).SetSerializer(new EnumSerializer<TicketLifeCycleType>(BsonType.String));
    }
}