using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.UserAccess.Domain.Users.Events
{
    public class UserPermissionsChangedDomainEvent : DomainEventBase
    {
        public UserId UserId { get; }

        public UserPermissionsChangedDomainEvent(UserId userId)
        {
            UserId = userId;
        }
    }
}
