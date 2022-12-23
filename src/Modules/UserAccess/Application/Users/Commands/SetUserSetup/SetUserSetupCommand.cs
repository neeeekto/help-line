using System;
using System.Collections.Generic;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserSetup
{
    public class SetUserSetupCommand : CommandBase
    {
        public Guid UserId { get; }
        public Dictionary<string, dynamic> Setup { get; }

        public SetUserSetupCommand(Guid userId, Dictionary<string, dynamic> setup)
        {
            UserId = userId;
            Setup = setup;
        }


    }
}
