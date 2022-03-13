using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.UserAccess.Domain.Users
{
    public class UserInfo : ValueObject
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Photo { get; }
        public string Language { get; }

        public UserInfo(string firstName, string lastName, string photo, string language)
        {
            FirstName = firstName;
            LastName = lastName;
            Photo = photo;
            Language = language;
        }
    }
}