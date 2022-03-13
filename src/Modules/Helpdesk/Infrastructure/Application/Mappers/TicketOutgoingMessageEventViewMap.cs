using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class TicketOutgoingMessageEventViewMap : BsonClassMap<TicketOutgoingMessageEventView>
    {
        public TicketOutgoingMessageEventViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(TicketOutgoingMessageEventView));
            MapMember(x => x.Message);
            MapMember(x => x.Recipients);
            MapMember(x => x.MessageId);
        }
    }
}