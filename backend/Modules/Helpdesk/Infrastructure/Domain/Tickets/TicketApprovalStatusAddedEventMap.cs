using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketApprovalStatusAddedEventMap : BsonClassMap<TicketApprovalStatusAddedEvent>
{
    public TicketApprovalStatusAddedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketApprovalStatusAddedEvent));
        MapMember(x => x.Message);
        MapMember(x => x.Status);
        MapMember(x => x.AuditId);

    }
}