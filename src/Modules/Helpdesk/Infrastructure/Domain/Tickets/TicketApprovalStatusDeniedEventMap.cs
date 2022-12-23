using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketApprovalStatusDeniedEventMap : BsonClassMap<TicketApprovalStatusDeniedEvent>
{
    public TicketApprovalStatusDeniedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketApprovalStatusDeniedEvent));
        MapMember(x => x.Message);
        MapMember(x => x.AuditId);

    }
}