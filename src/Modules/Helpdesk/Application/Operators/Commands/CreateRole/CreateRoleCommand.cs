using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.CreateRole
{
    public class CreateRoleCommand : CommandBase<Guid>
    {
        public OperatorRoleData RoleData { get; }

        public CreateRoleCommand(OperatorRoleData roleData)
        {
            RoleData = roleData;
        }
    }
}
