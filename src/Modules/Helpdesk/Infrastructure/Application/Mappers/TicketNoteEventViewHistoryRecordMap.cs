using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketNoteEventViewHistoryRecordMap : BsonClassMap<TicketNoteEventView.HistoryRecord>
    {
        public TicketNoteEventViewHistoryRecordMap()
        {
            AutoMap();
            MapMember(x => x.Date);
            MapMember(x => x.Initiator);
            MapMember(x => x.Message);
        }
    }
}