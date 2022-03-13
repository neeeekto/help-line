using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.UpdateRole
{
    public class UpdateRoleCommand : CommandBase
    {
        public Guid RoleId { get; }
        public OperatorRoleData Data { get; }

        public UpdateRoleCommand(Guid roleId, OperatorRoleData data)
        {
            RoleId = roleId;
            Data = data;
        }
    }
}
