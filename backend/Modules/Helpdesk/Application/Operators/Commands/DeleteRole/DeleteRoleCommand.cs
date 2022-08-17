using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.DeleteRole
{
    public class DeleteRoleCommand : CommandBase
    {
        public Guid RoleId { get; }

        public DeleteRoleCommand(Guid roleId)
        {
            RoleId = roleId;
        }
    }
}
