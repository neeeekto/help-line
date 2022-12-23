using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketAutoreplyEventMap : BsonClassMap<TicketAutoreplySendedEvent>
{
    public TicketAutoreplyEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketAutoreplySendedEvent));
        MapMember(x => x.Message);
        MapMember(x => x.Reminder).SetIgnoreIfNull(true);
        MapMember(x => x.Resolve);
        MapMember(x => x.Tags);
    }
}