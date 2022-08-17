using System;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Users.ViewsModels;

namespace HelpLine.Modules.UserAccess.Application.Users.Queries.GetUsers
{
    public class GetUserQuery : QueryBase<UserView>
    {
        public Guid UserId { get; }

        public GetUserQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
