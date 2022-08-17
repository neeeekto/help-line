using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketIncomingMessageEventViewMap : BsonClassMap<TicketIncomingMessageEventView>
    {
        public TicketIncomingMessageEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketIncomingMessageEventView));
            MapMember(x => x.Message);
            MapMember(x => x.Channel);
        }
    }
}