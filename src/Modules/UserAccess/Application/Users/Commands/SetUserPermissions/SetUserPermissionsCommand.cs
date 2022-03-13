using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserPermissions
{
    public class SetUserPermissionsCommand : CommandBase
    {
        public Guid UserId { get; }
        public IEnumerable<string> Permissions { get; }

        public SetUserPermissionsCommand(Guid userId, IEnumerable<string> permissions)
        {
            UserId = userId;
            Permissions = permissions;
        }


    }
}
