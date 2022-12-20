using HelpLine.Modules.UserAccess.Application.Identity;
using MongoDB.Bson.Serialization;

namespace HelpLine.Modules.UserAccess.Infrastructure.Application.Identity
{
    internal class UserSessionMap : BsonClassMap<UserSession>
    {
        public UserSessionMap()
        {
            MapIdField(x => x.SessionId);
            AutoMap();
        }
    }
}
