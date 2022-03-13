using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketAssigmentBindingEventViewMap : BsonClassMap<TicketAssigmentBindingEventView>
    {
        public TicketAssigmentBindingEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketAssigmentBindingEventView));
            MapMember(x => x.HardAssigment);
        }
    }
}