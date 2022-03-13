using System;
using HelpLine.Modules.UserAccess.Application.Contracts;
using Newtonsoft.Json;

namespace HelpLine.Modules.UserAccess.Application.Identity.Commands.RemoveUserSessions
{
    public class RemoveUserSessionsCommand : CommandBase
    {
        public Guid UserId { get; }

        public RemoveUserSessionsCommand(Guid userId)
        {
            UserId = userId;
        }
    }

    public class RemoveUserSessionCommand : CommandBase
    {
        public Guid SessionId { get; }

        [JsonConstructor]
        public RemoveUserSessionCommand(Guid sessionId)
        {
            SessionId = sessionId;
        }
    }
}
