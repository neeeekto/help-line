using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.IntegrationTests;
using MongoDB.Driver;

namespace HelpLine.Services.Jobs.Tests.SeedWork
{
    public abstract class JobTestsBase : TestBase
    {
        protected Guid UserId = Guid.NewGuid();
        protected JobsService Service = new JobsService();
        protected JobsStartup Startup;

        protected override IExecutionContextAccessor GetExecutionCtx()
        {
            return new ExecutionContextMock()
            {
                UserId = UserId,
                CorrelationId = UserId,
                IsAvailable = true,
                Project = "test"
            };
        }

        protected override void SetupOther()
        {
            Startup = JobsStartup.Initialize(
                new JobsStartupConfig()
                {
                    ConnectionString = ConnectionString,
                    DbName = DbName,
                    Queue = BusServiceFactory.MakeQueue("jobs"),
                    Logger = Logger,
                    ContextAccessor = new ExecutionContextMock(),
                    JobTasksAssemblie = new[]
                    {
                        typeof(TestJobTask).Assembly,
                    }
                });
        }

        protected async override Task ClearDatabase(MongoConnection db)
        {
            var collections = await db.Database.ListCollectionNamesAsync();
            await collections.ForEachAsync(x => db.Database.DropCollection(x));
        }
    }
}
