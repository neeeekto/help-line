using System.Collections.Generic;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Domain.Roles;
using HelpLine.Modules.UserAccess.Domain.Users;

namespace HelpLine.Modules.UserAccess.Infrastructure.Domain.Users
{
    internal class UserMap : BsonClassMap<User>
    {
        public UserMap()
        {
            AutoMap();
            MapIdMember(x => x.Id);
        }
    }

    internal class UserSecurityMap : BsonClassMap<UserSecurity>
    {
        public UserSecurityMap()
        {
            AutoMap();
            MapField("_password").SetElementName("Password");
        }
    }

    internal class UserInfoMap : BsonClassMap<UserInfo>
    {
        public UserInfoMap()
        {
            AutoMap();
        }
    }

    internal class UserRolesMap : BsonClassMap<UserRoles>
    {
        public UserRolesMap()
        {
            AutoMap();
            MapField("_global").SetElementName("Global");
            MapField("_byProject").SetElementName("ByProject").SetSerializer(
                new DictionaryInterfaceImplementerSerializer<Dictionary<ProjectId, List<RoleId>>>(
                    DictionaryRepresentation.ArrayOfDocuments));
        }
    }
}
