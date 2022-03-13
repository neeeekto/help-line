using System;
using HelpLine.BuildingBlocks.Application.Events;
using HelpLine.Modules.UserAccess.Domain.Users.Events;
using Newtonsoft.Json;

namespace HelpLine.Modules.UserAccess.Application.Users.Notifications
{
    internal class UserCreatedNotification : DomainNotificationBase<UserCreatedDomainEvent>
    {
        [JsonConstructor]
        public UserCreatedNotification(UserCreatedDomainEvent domainEvent, Guid id) : base(domainEvent, id)
        {
        }
    }
}
