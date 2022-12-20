using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketReminderScheduledEventMap : BsonClassMap<TicketReminderScheduledEvent>
{
    public TicketReminderScheduledEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketReminderScheduledEvent));
        MapMember(x => x.Reminder);
        MapMember(x => x.ExecutionDate);
    }
}