using MongoDB.Bson.Serialization;
using HelpLine.Modules.UserAccess.Domain.Roles;

namespace HelpLine.Modules.UserAccess.Infrastructure.Domain.Roles
{
    internal class RoleMap : BsonClassMap<Role>
    {
        public RoleMap()
        {
            AutoMap();
            MapIdMember(x => x.Id);
            MapField("_permissions").SetElementName("Permissions");
        }
    }
}