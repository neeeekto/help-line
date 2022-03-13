using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Contracts;
using HelpLine.Modules.UserAccess.Application.Users.DTO;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : CommandBase<Guid>
    {
        public UserInfoDto Info { get; }
        public string Email { get; }

        public IEnumerable<string> Permissions { get; }
        public IEnumerable<Guid> GlobalRoles { get; }
        public IDictionary<string, IEnumerable<Guid>> ProjectsRoles { get; }
        public IEnumerable<string> Projects { get; }

        public CreateUserCommand(UserInfoDto info, string email, IEnumerable<Guid> globalRoles,
            IDictionary<string, IEnumerable<Guid>> projectsRoles, IEnumerable<string> permissions,
            IEnumerable<string> projects)
        {
            Info = info;
            Email = email;
            GlobalRoles = globalRoles;
            ProjectsRoles = projectsRoles;
            Permissions = permissions;
            Projects = projects;
        }


    }
}
