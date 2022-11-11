using System;
using HelpLine.BuildingBlocks.Application.Events;
using HelpLine.Modules.UserAccess.Domain.Roles.Events;

namespace HelpLine.Modules.UserAccess.Application.Roles.Notifications
{
    public class RolePermissionsChangedNotificationEvent : DomainNotificationBase<RolePermissionsChangedDomainEvent>
    {
        public RolePermissionsChangedNotificationEvent(RolePermissionsChangedDomainEvent domainEvent, Guid id) : base(
            domainEvent, id)
        {
        }
    }
}
