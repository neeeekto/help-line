using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketTimerCanceledEventMap : BsonClassMap<TicketLifecycleCanceledEvent>
{
    public TicketTimerCanceledEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketLifecycleCanceledEvent));
        MapMember(x => x.LifeCycleType).SetSerializer(new EnumSerializer<TicketLifeCycleType>(BsonType.String));
    }
}