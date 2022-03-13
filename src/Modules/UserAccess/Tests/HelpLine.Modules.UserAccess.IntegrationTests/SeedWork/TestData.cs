using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Users.DTO;

namespace HelpLine.Modules.UserAccess.IntegrationTests.SeedWork
{
    public class TestData
    {
        public UserInfoDto UserInfo = new UserInfoDto
        {
            Language = "en",
            Photo = "en",
            FirstName = "fn",
            LastName = "ln"
        };

        public string Email = "test@te.te";
        public IEnumerable<string> Permissions = new string[] { };
        public IEnumerable<Guid> GlobalRoles = new Guid[] { };
        public Dictionary<string, IEnumerable<Guid>> ProjectsRoles = new Dictionary<string, IEnumerable<Guid>>();
    }
}
