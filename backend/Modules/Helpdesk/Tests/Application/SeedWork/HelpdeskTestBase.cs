using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Application.Emails;
using HelpLine.BuildingBlocks.IntegrationTests;
using HelpLine.Modules.Helpdesk.Application.Projects.Commands.CreateProject;
using HelpLine.Modules.Helpdesk.Application.Projects.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Commands.CreateTicket;
using HelpLine.Modules.Helpdesk.Infrastructure;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration;
using HelpLine.Modules.UserAccess.IntegrationEvents;
using HelpLine.Services.Jobs;
using HelpLine.Services.Jobs.Contracts;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Tests.Application.SeedWork
{
    public abstract class HelpdeskTestBase : TestBase<HelpdeskModule>
    {
        protected static Guid UserId = Guid.Parse("d714086f-fa98-47a5-8b36-77c02d7ee0af");

        protected abstract string NS { get; }

        protected override string DbName => $"hlit_{NS}";
        public const string ProjectId = "test";
        public const string Tag = "test";
        public Guid OperatorId => UserId;
        protected const string TestStr = "test";
        protected const string EngLangKey = "en";
        public TemplateRendererMock TemplateRenderer = new TemplateRendererMock();
        public IEmailSender EmailSender = new EmailSenderMock();
        protected IJobTaskQueueFactory JobTaskQueueFactory { get; private set; }
        protected IJobTaskQueue JobTaskQueue { get; private set; }

        protected override HelpdeskModule InitModule()
        {
            JobTaskQueueFactory = new JobTaskQueueFactory(BusServiceFactory);
            JobTaskQueue = JobTaskQueueFactory.MakeQueue("Jobs");

            var startup = HelpdeskStartup.Initialize(
                ConnectionString,
                DbName,
                BusServiceFactory,
                BusServiceFactory,
                JobTaskQueue,
                ExecutionContext,
                Logger,
                TemplateRenderer, EmailSender);
            startup.EnableAppQueueHandling();
            startup.EnableJobHandling();

            return new HelpdeskModule();
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

        protected async Task CreateOperator(Guid? operatorId = null)
        {
            var evt = new NewUserCreatedIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow, operatorId ?? OperatorId, "test", "test",
                "test");
            BusServiceFactory.PublishInEventBus(evt);
            await BusServiceFactory.EmitAllEvents();
            await BusServiceFactory.EmitAllQueues();
        }

        protected async Task CreateProject(string[]? languages = null, string? projectId = null)
        {
            languages ??= new[] {EngLangKey};
            await Module.ExecuteCommandAsync(new CreateProjectCommand(projectId ?? ProjectId, new ProjectDataDto( "test", "img", languages )));
            await BusServiceFactory.EmitAllQueues();
        }
        protected async Task CreateProject(string? projectId)
        {
            await CreateProject(null, projectId);
        }



        protected async Task<string> CreateTicket(TicketTestData testData)
        {
            var cmd = new CreateTicketCommand(testData.ProjectId, testData.Language, testData.Initiator,
                testData.Tags, testData.Channels, testData.UserMeta,
                null, testData.Message, testData.Source, null);
            return await Module.ExecuteCommandAsync(cmd);
        }
    }
}
