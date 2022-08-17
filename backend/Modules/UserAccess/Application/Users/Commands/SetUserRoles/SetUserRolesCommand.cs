using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserRoles
{
    public class SetUserRolesCommand : CommandBase
    {
        public Guid UserId { get; }
        public IEnumerable<Guid> GlobalRoles { get; }
        public IDictionary<string, IEnumerable<Guid>> ProjectsRoles { get; }

        public SetUserRolesCommand(Guid userId, IEnumerable<Guid> globalRoles,
            IDictionary<string, IEnumerable<Guid>> projectsRoles)
        {
            UserId = userId;
            GlobalRoles = globalRoles;
            ProjectsRoles = projectsRoles;
        }


    }
}
