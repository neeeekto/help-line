using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.IntegrationTests;
using MongoDB.Driver;

namespace HelpLine.Services.TemplateRenderer.Tests.SeedWork
{
    public abstract class TemplateRendererTestsBase : TestBase
    {
        protected Guid UserId = Guid.NewGuid();
        protected TemplateRendererService Service = new TemplateRendererService();
        protected TemplateRendererStartup Startup;

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
            Startup = TemplateRendererStartup.Initialize(
                    ConnectionString,
                    DbName,
                    Logger,
                    new ExecutionContextMock())
                ;
        }

        protected async override Task ClearDatabase(MongoConnection db)
        {
            var collections = await db.Database.ListCollectionNamesAsync();
            await collections.ForEachAsync(x => db.Database.DropCollection(x));
        }
    }
}
