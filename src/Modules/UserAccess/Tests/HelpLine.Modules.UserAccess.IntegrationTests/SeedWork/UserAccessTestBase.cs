using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.IntegrationTests;
using HelpLine.Modules.UserAccess.Infrastructure;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration;
using HelpLine.Services.Jobs;
using HelpLine.Services.Jobs.Contracts;
using MongoDB.Driver;

namespace HelpLine.Modules.UserAccess.IntegrationTests.SeedWork
{
    public abstract class UserAccessTestBase : TestBase<UserAccessModule>
    {
        protected static Guid UserId = Guid.Parse("d714086f-fa98-47a5-8b36-77c02d7ee0af");
        protected abstract string NS { get; }

        protected override string DbName => $"helpline-tests_useraccess_{NS}";
        protected IJobTaskQueueFactory JobTaskQueueFactory { get; private set; }
        protected IJobTaskQueue JobTaskQueue { get; private set; }
        protected const string JobTaskQueueKey = "Jobs";

        protected override UserAccessModule InitModule()
        {
            JobTaskQueueFactory = new JobTaskQueueFactory(BusServiceFactory);
            JobTaskQueue = JobTaskQueueFactory.MakeQueue(JobTaskQueueKey);

            var startup = UserAccessStartup.Initialize(
                ConnectionString,
                DbName,
                BusServiceFactory,
                BusServiceFactory,
                ExecutionContext,
                StorageFactory,
                JobTaskQueue,
                Logger);
            startup.EnableAppQueueHandling().EnableJobHandling();

            return new UserAccessModule();
        }

        protected override IExecutionContextAccessor GetExecutionCtx()
        {
            return new ExecutionContextMock(UserId);
        }

        protected override async Task ClearDatabase(MongoConnection db)
        {
            var collections = await db.Database.ListCollectionNamesAsync();
            await collections.ForEachAsync(x => db.Database.DropCollection(x));
        }
    }
}
