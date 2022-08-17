using System;
using HelpLine.BuildingBlocks.Bus.EventsBus;

namespace HelpLine.Modules.UserAccess.IntegrationEvents
{
    public class UserRemovedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; }
        public UserRemovedIntegrationEvent(Guid id, DateTime occurredOn, Guid userId) : base(id, occurredOn)
        {
            UserId = userId;
        }
    }
}
