using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.IntegrationTests;
using HelpLine.Modules.UserAccess.Application.Identity;
using HelpLine.Modules.UserAccess.Application.Identity.Commands.RemoveUserSessions;
using HelpLine.Modules.UserAccess.Application.Identity.Commands.SaveUserSession;
using HelpLine.Modules.UserAccess.Application.Identity.Queries.GetUserSession;
using HelpLine.Modules.UserAccess.Jobs;
using HelpLine.Modules.UserAccess.Tests.Application.SeedWork;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace HelpLine.Modules.UserAccess.Tests.Application.Jobs
{
    [TestFixture]
    [NonParallelizable]
    public class SyncSessionsJobTests : UserAccessTestBase
    {
        protected override string NS => nameof(SyncSessionsJobTests);

        [Test]
        public async Task RestoreAfterBrokenCache_Success()
        {

            var userId = Guid.NewGuid();

            // Broke all methods. Cache is not available
            var cache = (InMemoryStorage<UserSession> )StorageFactory.CacheStorages.FirstOrDefault().Value;
            cache.Set(Arg.Any<object>(), Arg.Any<object>(), Arg.Any<TimeSpan>()).ThrowsForAnyArgs(new ApplicationException());
            var sessionId = await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, "", TimeSpan.FromDays(100)));

            // Cache has been restored
            cache.ClearSubstitute();

            await BusServiceFactory.PublishInQueues(new SyncSessionsJob(Guid.NewGuid()));
            var result = await Module.ExecuteQueryAsync(new GetUserSessionQuery(sessionId));

            Assert.That(result, Is.Not.Null);

        }

        [Test]
        public async Task DeleteAllAfterBrokenCache_Success()
        {

            var userId = Guid.NewGuid();

            var sessionId = await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, "", TimeSpan.FromDays(100)));

            // Broke all methods. Cache is not available
            var cache = (InMemoryStorage<UserSession> )StorageFactory.CacheStorages.FirstOrDefault().Value;
            cache.RemoveMany(Arg.Any<IEnumerable<object>>()).ThrowsForAnyArgs(new ApplicationException());
            cache.RemoveOne(Arg.Any<object>()).ThrowsForAnyArgs(new ApplicationException());

            await Module.ExecuteCommandAsync(new RemoveUserSessionCommand(sessionId));

            // Cache has been restored
            cache.ClearSubstitute();

            await BusServiceFactory.PublishInQueues(new SyncSessionsJob(Guid.NewGuid()));
            var result = await Module.ExecuteQueryAsync(new GetUserSessionQuery(sessionId));

            Assert.That(result, Is.Null);

        }

        [Test]
        public async Task LiveSessionWontBeRemoved_Success()
        {

            var userId = Guid.NewGuid();

            var sessionId = await Module.ExecuteCommandAsync(new SaveUserSessionCommand(userId, "", TimeSpan.FromDays(100)));
            var resultBefore = await Module.ExecuteQueryAsync(new GetUserSessionQuery(sessionId));
            await BusServiceFactory.PublishInQueues(new SyncSessionsJob(Guid.NewGuid()));
            var resultAfter = await Module.ExecuteQueryAsync(new GetUserSessionQuery(sessionId));


            Assert.That(resultAfter, Is.Not.Null);
            Assert.That(resultAfter.Data, Is.EqualTo(resultBefore.Data));
            Assert.That(resultAfter.Expired.ToLongDateString(), Is.EqualTo(resultBefore.Expired.ToLongDateString()));
            Assert.That(resultAfter.Expired.ToLongTimeString(), Is.EqualTo(resultBefore.Expired.ToLongTimeString()));

            Assert.That(resultAfter.CreateDate.ToLongDateString(), Is.EqualTo(resultBefore.CreateDate.ToLongDateString()));
            Assert.That(resultAfter.CreateDate.ToLongTimeString(), Is.EqualTo(resultBefore.CreateDate.ToLongTimeString()));
            Assert.That(resultAfter.SessionId, Is.EqualTo(resultBefore.SessionId));
            Assert.That(resultAfter.UserId, Is.EqualTo(resultBefore.UserId));

        }
    }
}
