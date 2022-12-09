using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketApprovalStatusApprovedEventMap : BsonClassMap<TicketApprovalStatusApprovedEvent>
{
    public TicketApprovalStatusApprovedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketApprovalStatusApprovedEvent));
        MapMember(x => x.AuditId);

    }
}