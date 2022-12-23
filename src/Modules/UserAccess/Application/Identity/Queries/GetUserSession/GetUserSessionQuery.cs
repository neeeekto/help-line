using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Identity.Queries.GetUserSession
{
    public class GetUserSessionQuery : QueryBase<UserSession?>
    {
        public Guid SessionId { get; }

        public GetUserSessionQuery(Guid sessionId)
        {
            SessionId = sessionId;
        }
    }

    public class GetUserSessionsQuery : QueryBase<IEnumerable<UserSession>>
    {
        public Guid UserId { get; }

        public GetUserSessionsQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
