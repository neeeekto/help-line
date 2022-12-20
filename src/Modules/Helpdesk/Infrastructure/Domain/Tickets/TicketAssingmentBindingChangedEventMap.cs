using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketAssingmentBindingChangedEventMap : BsonClassMap<TicketAssingmentBindingChangedEvent>
{
    public TicketAssingmentBindingChangedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketAssingmentBindingChangedEvent));
        MapMember(x => x.HardAssigment);
    }
}