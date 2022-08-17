using System;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Roles.Commands.RemoveRole
{
    public class RemoveRoleCommand : CommandBase
    {
        public Guid RoleId { get; }

        public RemoveRoleCommand(Guid roleId)
        {
            RoleId = roleId;
        }


    }
}
