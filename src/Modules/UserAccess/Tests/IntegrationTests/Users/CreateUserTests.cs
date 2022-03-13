using System;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;
using HelpLine.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using HelpLine.Modules.UserAccess.Application.Users.CreateUser;
using HelpLine.Modules.UserAccess.Application.Users.GetUser;
using HelpLine.Modules.UserAccess.Domain.UserRegistrations;
using CompanyNames.MyMeetings.Modules.UserAccess.IntegrationTests.SeedWork;
using CompanyNames.MyMeetings.Modules.UserAccess.IntegrationTests.UserRegistrations;
using NUnit.Framework;

namespace CompanyNames.MyMeetings.Modules.UserAccess.IntegrationTests.Users
{
    [TestFixture]
    public class CreateUserTests : TestBase
    {
        [Test]
        public async Task CreateUser_Test()
        {
            var registrationId = await UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
                UserRegistrationSampleData.Login,
                UserRegistrationSampleData.Password,
                UserRegistrationSampleData.Email,
                UserRegistrationSampleData.FirstName,
                UserRegistrationSampleData.LastName));
            await UserAccessModule.ExecuteCommandAsync(new ConfirmUserRegistrationCommand(registrationId));

            var userId = await UserAccessModule.ExecuteCommandAsync(new CreateUserCommand(Guid.NewGuid(), new UserRegistrationId(registrationId)));

            var user = await UserAccessModule.ExecuteQueryAsync(new GetUserQuery(userId));

            Assert.That(user.Login, Is.EqualTo(UserRegistrationSampleData.Login));
        }
    }
}