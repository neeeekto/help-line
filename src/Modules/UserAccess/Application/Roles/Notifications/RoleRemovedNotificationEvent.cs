using System;
using HelpLine.BuildingBlocks.Application.Events;
using HelpLine.Modules.UserAccess.Domain.Roles.Events;

namespace HelpLine.Modules.UserAccess.Application.Roles.Notifications
{
    public class RoleRemovedNotificationEvent : DomainNotificationBase<RoleRemovedDomainEvent>
    {
        public RoleRemovedNotificationEvent(RoleRemovedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
        {
        }
    }
}
