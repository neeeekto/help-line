using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketReminderCanceledEventMap : BsonClassMap<TicketReminderCanceledEvent>
{
    public TicketReminderCanceledEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketReminderCanceledEvent));
        MapMember(x => x.ReminderId);
    }
}