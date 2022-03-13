using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : CommandBase<Guid>
    {
        public string Name { get; }
        public IEnumerable<string> Permissions { get; }

        public CreateRoleCommand(string name, IEnumerable<string> permissions)
        {
            Name = name;
            Permissions = permissions;
        }


    }
}
