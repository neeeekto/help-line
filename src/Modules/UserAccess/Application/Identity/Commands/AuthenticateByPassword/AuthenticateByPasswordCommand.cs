using System;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Identity.Commands.AuthenticateByPassword
{
    public class AuthenticateByPasswordCommand : CommandBase<Guid?>
    {
        public string Email { get; }
        public string Password { get; }

        public AuthenticateByPasswordCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
