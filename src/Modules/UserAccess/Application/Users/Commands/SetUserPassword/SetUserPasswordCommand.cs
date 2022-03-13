using System;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserPassword
{
    public class SetUserPasswordCommand : CommandBase
    {
        public Guid UserId { get; }
        public string? Password { get; }

        public SetUserPasswordCommand(Guid userId, string? password)
        {
            UserId = userId;
            Password = password;
        }


    }
}
