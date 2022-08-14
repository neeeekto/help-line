using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.IntegrationTests;
using HelpLine.Modules.UserAccess.Application.Identity;
using HelpLine.Modules.UserAccess.Application.Identity.Commands.RemoveUserSessions;
using HelpLine.Modules.UserAccess.Application.Identity.Commands.SaveUserSession;
using HelpLine.Modules.UserAccess.Application.Identity.Queries.GetUserSession;
using HelpLine.Modules.UserAccess.Tests.Application.SeedWork;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace HelpLine.Modules.UserAccess.Tests.Application.Identity
{
    [TestFixture]
    [NonParallelizable]
    public class RemoveUserSessionsTests : UserAccessTestBase
    {
        protected override string NS => nameof(RemoveUserSessionsTests);

        [Test]
        public async Task RemoveSessionBySessionId_Success()
        {
            var testData = "test";
            var userId = Guid.NewGuid();
            var sessionId =
                await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, testData, TimeSpan.FromDays(100)));
            var sessionId2 =
                await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, testData, TimeSpan.FromDays(100)));
            await Module.ExecuteCommandAsync(new RemoveUserSessionCommand(sessionId));
            var result = await Module.ExecuteQueryAsync(new GetUserSessionQuery(sessionId));
            var otherSession = await Module.ExecuteQueryAsync(new GetUserSessionQuery(sessionId2));

            Assert.That(result, Is.Null);
            Assert.That(otherSession, Is.Not.Null);
        }

        [Test]
        public async Task RemoveSessionByUserId_Success()
        {
            var testData = "test";
            var userId = Guid.NewGuid();
            var userIdOther = Guid.NewGuid();
            await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, testData, TimeSpan.FromDays(100)));
            await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, testData, TimeSpan.FromDays(100)));
            var sessionIdOther =
                await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userIdOther, testData,
                    TimeSpan.FromDays(100)));
            await Module.ExecuteCommandAsync(new RemoveUserSessionsCommand(userId));
            var result = await Module.ExecuteQueryAsync(new GetUserSessionsQuery(userId));
            var sessionOther = await Module.ExecuteQueryAsync(new GetUserSessionQuery(sessionIdOther));
            Assert.That(result.Any(), Is.False);
            Assert.That(sessionOther, Is.Not.Null);
        }


        [Test]
        public async Task RemoveNotExistsSession_Success()
        {
            await Module.ExecuteCommandAsync(new RemoveUserSessionCommand(Guid.NewGuid()));
            Assert.That(true, Is.True);
        }

        [Test]
        public async Task RemoveSessionWithBrokenCache_Success()
        {
            var testData = "test";
            var userId = Guid.NewGuid();
            await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, testData, TimeSpan.FromDays(100)));
            // Broke all methods. Cache is not available
            var cache = (InMemoryStorage<UserSession>)StorageFactory.CacheStorages.FirstOrDefault().Value;
            cache.RemoveMany(Array.Empty<object>()).ThrowsForAnyArgs(new ApplicationException());
            cache.Get(default).ThrowsForAnyArgs(new ApplicationException());

            await Module.ExecuteCommandAsync(new RemoveUserSessionsCommand(userId));
            var result = await Module.ExecuteQueryAsync(new GetUserSessionsQuery(userId));
            Assert.That(result.Any(), Is.False);
        }
    }
}
