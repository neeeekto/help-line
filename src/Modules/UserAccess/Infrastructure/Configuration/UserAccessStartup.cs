using Autofac;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration.Application;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration.DataAccess;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration.Domain;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration.EventsBus;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration.Jobs;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration.Logging;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration.Mapper;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration.Mediation;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration.Processing;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;
using HelpLine.Services.Jobs.Contracts;
using Serilog;
using Serilog.AspNetCore;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration
{
    public class UserAccessStartup
    {
        private static IContainer _container;

        public static UserAccessStartup Initialize(
            string connectionString,
            string dbName,
            IQueueFactory queueFactory,
            IEventBusFactory busFactory,
            IExecutionContextAccessor executionContextAccessor,
            IStorageFactory storageFactory,
            IJobTaskQueue jobTaskQueue,
            ILogger logger)
        {
            var moduleLogger = logger.ForContext("Module", "UserAccess");

            ConfigureCompositionRoot(
                connectionString,
                dbName,
                queueFactory,
                busFactory,
                executionContextAccessor,
                storageFactory,
                jobTaskQueue,
                logger);

            return new UserAccessStartup(moduleLogger);

        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            string dbName,
            IQueueFactory queueFactory,
            IEventBusFactory busFactory,
            IExecutionContextAccessor executionContextAccessor,
            IStorageFactory storageFactory,
            IJobTaskQueue jobTaskQueue,
            ILogger logger)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(logger));

            var loggerFactory = new SerilogLoggerFactory(logger);
            containerBuilder.RegisterModule(new DataAccessModule(connectionString, dbName, loggerFactory));
            containerBuilder.RegisterModule(new DomainModule());
            containerBuilder.RegisterModule(new JobsModule(jobTaskQueue));
            containerBuilder.RegisterModule(new ApplicationModule(storageFactory));
            containerBuilder.RegisterModule(new ProcessingModule(queueFactory));
            containerBuilder.RegisterModule(new EventsBusModule(busFactory));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new OutboxModule());
            containerBuilder.RegisterModule(new MapperModule());


            containerBuilder.RegisterInstance(executionContextAccessor);
            containerBuilder.RegisterInstance(storageFactory);

            _container = containerBuilder.Build();

            UserAccessCompositionRoot.SetContainer(_container);
        }

        private readonly ILogger _moduleLogger;

        internal UserAccessStartup(ILogger moduleLogger)
        {
            _moduleLogger = moduleLogger;
        }

        public UserAccessStartup EnableAppQueueHandling()
        {
            EventsBusStartup.Initialize(_moduleLogger);
            ProcessingStartup.Initialize(_moduleLogger);
            return this;
        }

        public UserAccessStartup EnableJobHandling()
        {
            JobsStartup.Initialize(_moduleLogger);
            return this;
        }
    }
}
