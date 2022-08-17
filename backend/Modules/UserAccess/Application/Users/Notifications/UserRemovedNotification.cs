using System;
using HelpLine.BuildingBlocks.Application.Events;
using HelpLine.Modules.UserAccess.Domain.Users.Events;
using Newtonsoft.Json;

namespace HelpLine.Modules.UserAccess.Application.Users.Notifications
{
    public class UserRemovedNotification : DomainNotificationBase<UserDeletedDomainEvent>
    {
        [JsonConstructor]
        public UserRemovedNotification(UserDeletedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
        {
        }
    }
}
