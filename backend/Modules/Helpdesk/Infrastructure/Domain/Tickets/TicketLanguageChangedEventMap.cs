using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketLanguageChangedEventMap : BsonClassMap<TicketLanguageChangedEvent>
{
    public TicketLanguageChangedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketLanguageChangedEvent));
        MapMember(x => x.Language);
    }
}