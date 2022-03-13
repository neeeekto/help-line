using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : CommandBase
    {
        public Guid RoleId { get; }
        public string Name { get; }
        public IEnumerable<string> Permissions { get; }

        public UpdateRoleCommand(Guid roleId, string name, IEnumerable<string> permissions)
        {
            RoleId = roleId;
            Name = name;
            Permissions = permissions;
        }
    }
}
