using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Domain.Users;

namespace HelpLine.Modules.UserAccess.Application.Users.Queries.GetUsersByRole
{
    internal class GetUsersByRoleQuery : QueryBase<IEnumerable<User>>
    {
        public Guid RoleId { get; }

        public GetUsersByRoleQuery(Guid roleId)
        {
            RoleId = roleId;
        }


    }
}
