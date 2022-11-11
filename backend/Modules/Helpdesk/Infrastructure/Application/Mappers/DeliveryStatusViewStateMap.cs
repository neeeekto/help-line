using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class DeliveryStatusViewStateMap : BsonClassMap<DeliveryStatusView>
    {
        public DeliveryStatusViewStateMap()
        {
            AutoMap();
            MapMember(x => x.Detail).SetIgnoreIfNull(true);
            MapMember(x => x.Status).SetSerializer(new EnumSerializer<MessageStatus>(BsonType.String));
            MapMember(x => x.Date);
        }
    }
}