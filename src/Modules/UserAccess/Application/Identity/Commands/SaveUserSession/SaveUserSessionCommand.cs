using System;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Identity.Commands.SaveUserSession
{
    public class SaveUserSessionCommand : CommandBase<Guid>
    {
        public Guid UserId { get; }
        public string Data { get; }
        public TimeSpan Lifetime { get; }

        public SaveUserSessionCommand(Guid userId, string data, TimeSpan lifetime)
        {
            UserId = userId;
            Data = data;
            Lifetime = lifetime;
        }
    }
}
