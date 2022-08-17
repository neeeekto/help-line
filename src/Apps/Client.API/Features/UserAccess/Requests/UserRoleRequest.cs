using System;
using System.Collections.Generic;

namespace HelpLine.Apps.Client.API.Features.UserAccess.Requests
{
    public class UserRoleRequest
    {
        public IEnumerable<Guid> GlobalRoles { get; set; }
        public IDictionary<string, IEnumerable<Guid>> ProjectRoles { get; set; }
    }
}
