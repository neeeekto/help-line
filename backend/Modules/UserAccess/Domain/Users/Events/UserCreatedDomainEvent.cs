using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.UserAccess.Domain.Users.Events
{
    public class UserCreatedDomainEvent : DomainEventBase
    {
        public UserId UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }

        public UserCreatedDomainEvent(UserId userId, string firstName, string lastName, string email)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}