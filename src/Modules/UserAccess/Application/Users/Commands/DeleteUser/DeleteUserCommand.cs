using System;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : CommandBase
    {
        public Guid UserId { get; }

        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }


    }
}
