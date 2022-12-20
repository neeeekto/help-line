using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Mappers
{
    internal class UserInitiatorViewMap : BsonClassMap<UserInitiatorView>
    {
        public UserInitiatorViewMap()
        {
            AutoMap();
            SetDiscriminator(nameof(UserInitiatorView));
            MapMember(x => x.UserId);
        }
    }
}