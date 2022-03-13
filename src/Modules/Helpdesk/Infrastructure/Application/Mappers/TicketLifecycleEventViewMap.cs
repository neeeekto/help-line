using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketLifecycleEventViewMap : BsonClassMap<TicketLifecycleEventView>
    {
        public TicketLifecycleEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketLifecycleEventView));
            MapMember(x => x.Result);
            MapMember(x => x.Type);
            MapMember(x => x.ExecutionDate);
        }
    }
}