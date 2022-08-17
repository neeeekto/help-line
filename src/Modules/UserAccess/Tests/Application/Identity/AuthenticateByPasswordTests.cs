using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Identity.Commands.AuthenticateByPassword;
using HelpLine.Modules.UserAccess.Application.Users.Commands.CreateUser;
using HelpLine.Modules.UserAccess.Application.Users.Commands.SetUserPassword;
using HelpLine.Modules.UserAccess.Application.Users.Queries.GetUsers;
using HelpLine.Modules.UserAccess.Tests.Application.SeedWork;
using NUnit.Framework;

namespace HelpLine.Modules.UserAccess.Tests.Application.Identity
{
    [TestFixture]
    [NonParallelizable]
    public class AuthenticateByPasswordTests : UserAccessTestBase
    {
        protected override string NS => nameof(AuthenticateByPasswordTests);

        protected const string Password = "test";

        [Test]
        public async Task CanAuthenticateByPassword_Success()
        {
            var testData = new TestData();

            var cmd = new CreateUserCommand(testData.UserInfo, testData.Email, testData.GlobalRoles,
                testData.ProjectsRoles, testData.Permissions, Array.Empty<string>());

            var userId = await Module.ExecuteCommandAsync(cmd);
            await Module.ExecuteCommandAsync(new SetUserPasswordCommand(userId, Password));
            var result = await Module.ExecuteCommandAsync(new AuthenticateByPasswordCommand(testData.Email, Password));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetCorrectUserIdAfterAuthenticate_Success()
        {
            var testData = new TestData();

            var cmd = new CreateUserCommand(testData.UserInfo, testData.Email, testData.GlobalRoles,
                testData.ProjectsRoles, testData.Permissions, Array.Empty<string>());

            var userId = await Module.ExecuteCommandAsync(cmd);
            await Module.ExecuteCommandAsync(new SetUserPasswordCommand(userId, Password));
            var result = await Module.ExecuteCommandAsync(new AuthenticateByPasswordCommand(testData.Email, Password));
            var user = await Module.ExecuteQueryAsync(new GetUserQuery((Guid)result));

            Assert.That(user.Id, Is.EqualTo(userId));
            Assert.That(user.Email, Is.EqualTo(testData.Email));
        }

        [Test]
        public async Task CantAuthenticateByPasswordForUserWithoutPassword_Success()
        {
            var testData = new TestData();

            var cmd = new CreateUserCommand(testData.UserInfo, testData.Email, testData.GlobalRoles,
                testData.ProjectsRoles, testData.Permissions, Array.Empty<string>());

            var userId = await Module.ExecuteCommandAsync(cmd);
            var result = await Module.ExecuteCommandAsync(new AuthenticateByPasswordCommand(testData.Email, Password));

            Assert.That(result, Is.Null);
        }
    }
}
