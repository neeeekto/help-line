using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.IntegrationTests;
using HelpLine.Modules.UserAccess.Application.Identity;
using HelpLine.Modules.UserAccess.Application.Identity.Commands.SaveUserSession;
using HelpLine.Modules.UserAccess.Application.Identity.Queries.GetUserSession;
using HelpLine.Modules.UserAccess.Application.Users.Commands.CreateUser;
using HelpLine.Modules.UserAccess.Tests.Application.SeedWork;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace HelpLine.Modules.UserAccess.Tests.Application.Identity
{
    [TestFixture]
    [NonParallelizable]
    public class SaveUserSessionTests : UserAccessTestBase
    {
        protected override string NS => nameof(SaveUserSessionTests);

        [Test]
        public async Task SessionIsSaved_Success()
        {
            var testData = new TestData();

            var cmd = new CreateUserCommand(testData.UserInfo, testData.Email, testData.GlobalRoles,
                testData.ProjectsRoles, testData.Permissions, Array.Empty<string>());
            var testStr = "testStr";

            var userId = await Module.ExecuteCommandAsync(cmd);
            var sessionId = await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, testStr, TimeSpan.FromDays(100)));
            var result = await Module.ExecuteQueryAsync(new GetUserSessionQuery(sessionId));

            Assert.That(sessionId, Is.Not.Null);
            Assert.That(sessionId, Is.EqualTo(result.SessionId));
            Assert.That(result.Data, Is.EqualTo(testStr));
            Assert.That(result.Expired, Is.GreaterThan(DateTime.UtcNow));
            Assert.That(result.CreateDate, Is.LessThan(DateTime.UtcNow));
            Assert.That(result.UserId, Is.EqualTo(userId));
        }

        [Test]
        public async Task SessionIsSavedIfCacheDontWork_Success()
        {
            var testData = new TestData();

            var cmd = new CreateUserCommand(testData.UserInfo, testData.Email, testData.GlobalRoles,
                testData.ProjectsRoles, testData.Permissions, Array.Empty<string>());
            var testStr = "testStr";
            var cache = (InMemoryStorage<UserSession> )StorageFactory.CacheStorages.FirstOrDefault().Value;

            // Broke all methods. Cache is not available
            cache.Set(default, default, default).ThrowsForAnyArgs(new ApplicationException());
            cache.Get(default).ThrowsForAnyArgs(new ApplicationException());

            var userId = await Module.ExecuteCommandAsync(cmd);
            var sessionId = await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, testStr, TimeSpan.FromDays(100)));
            var result = await Module.ExecuteQueryAsync(new GetUserSessionQuery(sessionId));

            Assert.That(sessionId, Is.Not.Null);
            Assert.That(result, Is.Not.Null);
            Assert.That(sessionId, Is.EqualTo(result.SessionId));
            Assert.That(result.Data, Is.EqualTo(testStr));
            Assert.That(result.Expired, Is.GreaterThan(DateTime.UtcNow));
            Assert.That(result.CreateDate, Is.LessThan(DateTime.UtcNow));
            Assert.That(result.UserId, Is.EqualTo(userId));
        }
    }
}
