using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.BuildingBlocks.Domain;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NSubstitute;
using RabbitMQ.Client;
using Serilog;

namespace HelpLine.BuildingBlocks.IntegrationTests
{
    public abstract class TestBase
    {
        protected IConfiguration Configuration { get; set; }
        protected string ConnectionString { get; set; }
        protected abstract string DbName { get; }

        protected BusServiceFactory BusServiceFactory { get; set; }
        protected InMemoryStorageFactory StorageFactory { get; set; }
        protected MongoConnection Db { get; set; }
        protected ILogger Logger { get; set; }


        protected IExecutionContextAccessor ExecutionContext { get; set; }


        [OneTimeSetUp]
        public async Task Init()
        {
            Configuration ??= GetConfiguration();
            ConnectionString = Configuration["ConnectionString"];
            if (ConnectionString == null)
                throw new ApplicationException(
                    $"Define connection string to integration tests database using environment variable or file 'test.settings.json': ConnectionString");

            Db = new MongoConnection(ConnectionString, DbName);
            StorageFactory = new InMemoryStorageFactory();
            await ClearDatabase(Db);
            Logger = Substitute.For<ILogger>();
            ExecutionContext = GetExecutionCtx();

            BusServiceFactory = new BusServiceFactory();
            SetupOther();
        }

        protected virtual void SetupOther()
        {
        }

        [TearDown]
        public async Task Cleanup()
        {
            StorageFactory.Clear();
            await ClearDatabase(Db);
        }

        protected abstract IExecutionContextAccessor GetExecutionCtx();


        protected static void AssertBrokenRule<TRule>(AsyncTestDelegate testDelegate)
            where TRule : class, IBusinessRule
        {
            var message = $"Expected {typeof(TRule).Name} broken rule";
            var businessRuleValidationException =
                Assert.CatchAsync<BusinessRuleValidationException>(testDelegate, message);
            if (businessRuleValidationException != null)
            {
                Assert.That(businessRuleValidationException.BrokenRule, Is.TypeOf<TRule>(), message);
            }
        }

        protected abstract Task ClearDatabase(MongoConnection db);

        protected virtual IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("test.settings.json", true)
                .AddEnvironmentVariables()
                .Build();
        }
    }

    public abstract class TestBase<TModule> : TestBase
    {
        protected TModule Module { get; set; }

        protected abstract TModule InitModule();

        protected override void SetupOther()
        {
            Module = InitModule();
        }
    }
}
