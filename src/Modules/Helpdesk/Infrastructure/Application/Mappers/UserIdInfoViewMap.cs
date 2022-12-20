using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class UserIdInfoViewMap : BsonClassMap<UserIdInfoView>
    {
        public UserIdInfoViewMap()
        {
            AutoMap();
            MapMember(x => x.Channel);
            MapMember(x => x.UseForDiscussion);
            MapMember(x => x.Type).SetSerializer(new EnumSerializer<UserIdType>(BsonType.String));
            MapMember(x => x.UserId);
        }
    }
}