using System.Collections.Generic;

namespace HelpLine.Apps.Client.API.Features.UserAccess.Requests
{
    public class RoleRequest
    {
        public string Name { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}