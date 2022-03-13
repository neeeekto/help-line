using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserProjects
{
    public class SetUserProjectsCommand : CommandBase
    {
        public Guid UserId { get; }
        public IEnumerable<string> Projects { get; }

        public SetUserProjectsCommand(Guid userId, IEnumerable<string> projects)
        {
            UserId = userId;
            Projects = projects;
        }
    }
}
