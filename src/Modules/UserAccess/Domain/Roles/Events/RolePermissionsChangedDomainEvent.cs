using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.SharedKernel;

namespace HelpLine.Modules.UserAccess.Domain.Roles.Events
{
    public class RolePermissionsChangedDomainEvent : DomainEventBase
    {
        public RoleId RoleId { get; }
        public IEnumerable<PermissionKey> Permissions { get; }

        public RolePermissionsChangedDomainEvent(RoleId roleId, IEnumerable<PermissionKey> permissions)
        {
            RoleId = roleId;
            Permissions = permissions;
        }
    }
}
