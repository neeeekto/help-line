using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Users.DTO;

namespace HelpLine.Apps.Admin.API.Controllers.Requests
{
    public class UserCreateRequest
    {
        public UserInfoDto Info { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public IEnumerable<string> Projects { get; set; }

    }
}
