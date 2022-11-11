using System;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace HelpLine.Modules.UserAccess.Application.Identity.Commands.ClearUserSessions
{
    internal class ClearUserSessionsCommand : InternalCommandBase
    {
        public Guid UserId { get; }

        [JsonConstructor]
        public ClearUserSessionsCommand(Guid id, Guid userId) : base(id)
        {
            UserId = userId;
        }


    }
}
