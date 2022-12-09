using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketNoteAddedEventMap : BsonClassMap<TicketNotePostedEvent>
{
    public TicketNoteAddedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketNotePostedEvent));
        MapMember(x => x.Message);
        MapMember(x => x.Tags);
        MapMember(x => x.NoteId);
    }
}