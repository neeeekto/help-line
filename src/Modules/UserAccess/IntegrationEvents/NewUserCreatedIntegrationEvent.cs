using System;
using HelpLine.BuildingBlocks.Bus.EventsBus;

namespace HelpLine.Modules.UserAccess.IntegrationEvents
{
    public class NewUserCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId {get; }

        public string Email { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public NewUserCreatedIntegrationEvent(Guid id, DateTime occurredOn, Guid userId, string email, string firstName, string lastName) : base(id, occurredOn)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
