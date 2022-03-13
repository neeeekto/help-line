using HelpLine.Modules.UserAccess.Domain.Users;

namespace HelpLine.Modules.UserAccess.Application.Users.DTO
{
    public class UserInfoDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public string Language { get; set; }

        internal UserInfo ToDomainEntity()
        {
            return new UserInfo(FirstName, LastName, Photo, Language);
        }
    }
}
