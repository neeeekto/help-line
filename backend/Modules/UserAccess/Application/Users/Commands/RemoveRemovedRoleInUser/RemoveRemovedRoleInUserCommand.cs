using System;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using Newtonsoft.Json;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.RemoveRemovedRoleInUser
{
    internal class RemoveRemovedRoleInUserCommand : InternalCommandBase
    {
        public Guid UserId { get; }
        public Guid RoleId { get; }

        [JsonConstructor]
        public RemoveRemovedRoleInUserCommand(Guid id, Guid userId, Guid roleId) : base(id)
        {
            UserId = userId;
            RoleId = roleId;
        }


    }
}
