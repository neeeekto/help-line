using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Users.ViewsModels;
using IdentityServer4.Extensions;

namespace HelpLine.Modules.UserAccess.Application.Users.Queries.GetUsers
{
    public class GetUsersQuery : QueryBase<IEnumerable<UserView>>
    {
        public string? ProjectId { get; }

        public GetUsersQuery(string? projectId = null)
        {
            ProjectId = projectId;
        }
    }
}
