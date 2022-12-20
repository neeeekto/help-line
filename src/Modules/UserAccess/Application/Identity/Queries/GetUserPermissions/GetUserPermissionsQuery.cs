using System;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Identity.Views;

namespace HelpLine.Modules.UserAccess.Application.Identity.Queries.GetUserPermissions
{
    public class GetUserPermissionsQuery : QueryBase<UserPermissionsView>
    {
        public Guid UserId { get; }

        public GetUserPermissionsQuery(Guid userId)
        {
            UserId = userId;
        }


    }
}
