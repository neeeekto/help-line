using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketScheduleEventBaseMap : BsonClassMap<TicketScheduleEventBase>
{
    public TicketScheduleEventBaseMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketScheduleEventBase));
        MapMember(x => x.ScheduleId);
    }
}