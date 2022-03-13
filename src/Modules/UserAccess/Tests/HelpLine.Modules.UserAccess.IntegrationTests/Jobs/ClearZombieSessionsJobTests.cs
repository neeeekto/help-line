using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Identity.Commands.SaveUserSession;
using HelpLine.Modules.UserAccess.Application.Identity.Queries.GetUserSession;
using HelpLine.Modules.UserAccess.Application.Users.Commands.CreateUser;
using HelpLine.Modules.UserAccess.IntegrationTests.SeedWork;
using HelpLine.Modules.UserAccess.Jobs;
using NUnit.Framework;

namespace HelpLine.Modules.UserAccess.IntegrationTests.Jobs
{
    [TestFixture]
    [NonParallelizable]
    public class ClearZombieSessionsJobTests : UserAccessTestBase
    {
        protected override string NS => nameof(ClearZombieSessionsJobTests);

        [Test]
        public async Task ClearExpiredSessions_Success()
        {
            var testData = new TestData();

            var cmd = new CreateUserCommand(testData.UserInfo, testData.Email, testData.GlobalRoles,
                testData.ProjectsRoles, testData.Permissions, Array.Empty<string>());

            var userId = await Module.ExecuteCommandAsync(cmd);
            var sessionId1 = await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, "", TimeSpan.FromDays(-100)));
            var sessionId2 = await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, "", TimeSpan.FromDays(-100)));
            await BusServiceFactory.PublishInQueues(new ClearZombieSessionsJob(Guid.NewGuid()));
            var result = await Module.ExecuteQueryAsync(new GetUserSessionsQuery(userId));

            Assert.That(result.Any(), Is.False);

        }

        [Test]
        public async Task LiveSessionsWontBeCleared_Success()
        {
            var testData = new TestData();

            var cmd = new CreateUserCommand(testData.UserInfo, testData.Email, testData.GlobalRoles,
                testData.ProjectsRoles, testData.Permissions, Array.Empty<string>());

            var userId = await Module.ExecuteCommandAsync(cmd);
            var sessionId = await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, "", TimeSpan.FromDays(100)));
            await BusServiceFactory.PublishInQueues(new ClearZombieSessionsJob(Guid.NewGuid()));
            var result = await Module.ExecuteQueryAsync(new GetUserSessionQuery(sessionId));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.SessionId, Is.EqualTo(sessionId));

        }
    }
}
