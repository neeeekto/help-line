using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Emails;
using HelpLine.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration;
using HelpLine.Modules.UserAccess.Application.UserRegistrations.GetUserRegistration;
using HelpLine.Modules.UserAccess.Application.UserRegistrations.RegisterNewUser;
using HelpLine.Modules.UserAccess.Application.UserRegistrations.SendUserRegistrationConfirmationEmail;
using HelpLine.Modules.UserAccess.Application.Users.CreateUser;
using HelpLine.Modules.UserAccess.Domain.UserRegistrations;
using CompanyNames.MyMeetings.Modules.UserAccess.IntegrationTests.SeedWork;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace CompanyNames.MyMeetings.Modules.UserAccess.IntegrationTests.UserRegistrations
{
    [TestFixture]
    public class ConfirmUserRegistrationTests : TestBase
    {
        [Test]
        public async Task ConfirmUserRegistration_Test()
        {
            var registrationId = await UserAccessModule.ExecuteCommandAsync(new RegisterNewUserCommand(
                UserRegistrationSampleData.Login,
                UserRegistrationSampleData.Password,
                UserRegistrationSampleData.Email,
                UserRegistrationSampleData.FirstName,
                UserRegistrationSampleData.LastName));

            await UserAccessModule.ExecuteCommandAsync(new ConfirmUserRegistrationCommand(registrationId));

            var userRegistration = await UserAccessModule.ExecuteQueryAsync(new GetUserRegistrationQuery(registrationId));

            Assert.That(userRegistration.StatusCode, Is.EqualTo(UserRegistrationStatus.Confirmed.Value));

            var userRegistrationConfirmedNotification = await GetLastOutboxMessage<UserRegistrationConfirmedNotification>();

            Assert.That(userRegistrationConfirmedNotification.DomainEvent.UserRegistrationId.Value, Is.EqualTo(registrationId));
        }
    }
}