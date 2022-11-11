using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.UserAccess.Domain.Users.Events
{
    public class UserBlockedDomainEvent : DomainEventBase
    {
        public UserId UserId { get; }

        public UserBlockedDomainEvent(UserId userId)
        {
            UserId = userId;
        }
    }
}
