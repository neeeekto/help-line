using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Users.DTO;

namespace HelpLine.Apps.Client.API.Features.UserAccess.Requests
{
    public class UserCreateRequest
    {
        public UserInfoDto Info { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public IEnumerable<Guid> GlobalRoles { get; set; }
        public IDictionary<string, IEnumerable<Guid>> ProjectRoles { get; set; }
        public IEnumerable<string> Projects { get; set; }

    }
}
