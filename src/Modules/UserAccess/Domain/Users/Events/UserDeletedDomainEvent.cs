using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.UserAccess.Domain.Users.Events
{
    public class UserDeletedDomainEvent : DomainEventBase
    {
        public UserDeletedDomainEvent(UserId userId)
        {
            UserId = userId;
        }

        public UserId UserId { get; }
    }
}