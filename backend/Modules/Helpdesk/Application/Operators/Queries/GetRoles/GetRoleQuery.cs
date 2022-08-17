using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Operators.Models;

namespace HelpLine.Modules.Helpdesk.Application.Operators.Queries.GetRoles
{
    public class GetRoleQuery : QueryBase<OperatorRole>
    {
        public Guid OperatorRoleId { get; }

        public GetRoleQuery(Guid operatorRoleId)
        {
            OperatorRoleId = operatorRoleId;
        }
    }
}
