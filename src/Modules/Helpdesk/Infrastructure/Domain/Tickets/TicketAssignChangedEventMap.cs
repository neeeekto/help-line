using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketAssignChangedEventMap : BsonClassMap<TicketAssignChangedEvent>
{
    public TicketAssignChangedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketAssignChangedEvent));
        MapMember(x => x.Assignee).SetIgnoreIfNull(true);
    }
}