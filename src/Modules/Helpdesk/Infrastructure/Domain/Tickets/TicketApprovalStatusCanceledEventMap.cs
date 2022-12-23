using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketApprovalStatusCanceledEventMap : BsonClassMap<TicketApprovalStatusCanceledEvent>
{
    public TicketApprovalStatusCanceledEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketApprovalStatusCanceledEvent));
        MapMember(x => x.AuditId);

    }
}