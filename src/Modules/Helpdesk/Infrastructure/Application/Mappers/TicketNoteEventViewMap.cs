using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketNoteEventViewMap : BsonClassMap<TicketNoteEventView>
    {
        public TicketNoteEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketNoteEventView));
            MapMember(x => x.History);
            MapMember(x => x.Message);
            MapMember(x => x.Tags);
            MapMember(x => x.DeleteTime);
            MapMember(x => x.NoteId);
        }
    }
}