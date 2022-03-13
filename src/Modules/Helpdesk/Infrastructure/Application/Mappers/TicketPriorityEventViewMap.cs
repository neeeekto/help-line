using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketPriorityEventViewMap : BsonClassMap<TicketPriorityEventView>
    {
        public TicketPriorityEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketPriorityEventView));
            MapMember(x => x.New);
            MapMember(x => x.Old);
        }
    }
}