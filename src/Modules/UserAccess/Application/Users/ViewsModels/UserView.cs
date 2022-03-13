using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Domain.Users;

namespace HelpLine.Modules.UserAccess.Application.Users.ViewsModels
{
    public class UserView
    {
        public Guid Id { get; internal set; }
        public string Email { get; internal set; }
        public UserInfoView Info { get; internal set; }
        public UserStatus Status { get; internal set; }
        public IEnumerable<string> Projects { get; internal set; }
    }
}
