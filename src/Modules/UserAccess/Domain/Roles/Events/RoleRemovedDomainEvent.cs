using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.UserAccess.Domain.Roles.Events
{
    public class RoleRemovedDomainEvent : DomainEventBase
    {
        public RoleId RoleId { get; }

        public RoleRemovedDomainEvent(RoleId roleId)
        {
            RoleId = roleId;
        }
    }
}
