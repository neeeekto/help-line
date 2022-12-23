using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Commands.SetOperatorRoles
{
    public class SetOperatorRolesCommand : CommandBase
    {
        public Guid OperatorId { get; }
        public string ProjectId { get; }
        public IEnumerable<Guid> RoleIds { get; }

        public SetOperatorRolesCommand(Guid operatorId, string projectId, IEnumerable<Guid> roleIds)
        {
            OperatorId = operatorId;
            ProjectId = projectId;
            RoleIds = roleIds;
        }
    }
}
