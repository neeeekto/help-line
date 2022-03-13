﻿using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Emails;
using HelpLine.BuildingBlocks.Infrastructure.Emails;
using HelpLine.Modules.UserAccess.Application.UserRegistrations.SendUserRegistrationConfirmationEmail;
using HelpLine.Modules.UserAccess.Domain.UserRegistrations;
using CompanyNames.MyMeetings.Modules.UserAccess.IntegrationTests.SeedWork;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace CompanyNames.MyMeetings.Modules.UserAccess.IntegrationTests.UserRegistrations
{
    [TestFixture]
    public class SendUserRegistrationConfirmationEmailTests : TestBase
    {
        [Test]
        public async Task SendUserRegistrationConfirmationEmail_Test()
        {
            var registrationId = Guid.NewGuid();

            await UserAccessModule.ExecuteCommandAsync(new SendUserRegistrationConfirmationEmailCommand(
                Guid.NewGuid(),
                new UserRegistrationId(registrationId), 
                UserRegistrationSampleData.Email));

            var email = new EmailMessage(UserRegistrationSampleData.Email, 
                "MyMeetings - Please confirm your registration",
                "This should be link to confirmation page. For now, please execute HTTP request " +
                $"PATCH http://localhost:5000/userAccess/userRegistrations/{registrationId}/confirm");
            
            EmailSender.Received(Quantity.Exactly(1)).SendEmail(email);
        }
    }
}