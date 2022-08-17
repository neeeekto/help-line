using System;
using System.Collections.Generic;

namespace HelpLine.Modules.UserAccess.Application.Roles.ViewsModels
{
    public class RoleView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
