using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketTimerProlongatedEventMap : BsonClassMap<TicketLifecycleProlongatedEvent>
{
    public TicketTimerProlongatedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketLifecycleProlongatedEvent));
        MapMember(x => x.NextDate);
        MapMember(x => x.ScheduleId);
        MapMember(x => x.LifeCycleType).SetSerializer(new EnumSerializer<TicketLifeCycleType>(BsonType.String));
    }
}