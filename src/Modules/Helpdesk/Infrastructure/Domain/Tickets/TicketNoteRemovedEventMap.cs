using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Domain.Tickets;

internal class TicketNoteRemovedEventMap : BsonClassMap<TicketNoteRemovedEvent>
{
    public TicketNoteRemovedEventMap()
    {
        AutoMap();
        SetDiscriminator(nameof(TicketNoteRemovedEvent));
        MapMember(x => x.NoteId);
    }
}