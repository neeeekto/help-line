using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Domain.UnitTests.SeedWork;
using HelpLine.Modules.UserAccess.Domain.Users;
using NSubstitute;

namespace HelpLine.Modules.UserAccess.Domain.UnitTests.Users
{
    public class UserTestBase : TestBase
    {
        public IUsersChecker UsersChecker = Substitute.For<IUsersChecker>();
        public IUserRepository UserRepository = Substitute.For<IUserRepository>();

        protected void ClearServices()
        {
            UsersChecker = Substitute.For<IUsersChecker>();
            UsersChecker.CheckEmail(default).ReturnsForAnyArgs(Task.FromResult(false));

        }
    }
}
