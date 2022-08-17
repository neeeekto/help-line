using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketAssigmentEventViewMap : BsonClassMap<TicketAssigmentEventView>
    {
        public TicketAssigmentEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketAssigmentEventView));
            MapMember(x => x.From);
            MapMember(x => x.To);
        }
    }
}