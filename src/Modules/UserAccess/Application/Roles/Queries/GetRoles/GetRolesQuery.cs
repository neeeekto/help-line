using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Roles.ViewsModels;

namespace HelpLine.Modules.UserAccess.Application.Roles.Queries.GetRoles
{
    public class GetRolesQuery : QueryBase<IEnumerable<RoleView>>
    {
    }
}
