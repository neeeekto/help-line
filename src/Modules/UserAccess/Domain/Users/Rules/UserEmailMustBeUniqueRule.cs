using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.Modules.UserAccess.Domain.Users.Rules
{
    public class UserEmailMustBeUniqueRule : IBusinessRuleAsync
    {
        private readonly IUsersChecker _usersChecker;
        private readonly string _email;

        internal UserEmailMustBeUniqueRule(IUsersChecker usersChecker, string email)
        {
            _usersChecker = usersChecker;
            _email = email;
        }

        public Task<bool> IsBroken() => _usersChecker.CheckEmail(_email).ContinueWith(x => !x.Result);

        public string Message => "User email must be unique";
    }
}
