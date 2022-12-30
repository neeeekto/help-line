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
            UserAccessStartupConfig config)
        {
            var moduleLogger = config.Logger.ForContext("Module", "UserAccess");

            ConfigureCompositionRoot(
                config);

            return new UserAccessStartup(moduleLogger);
        }

        private static void ConfigureCompositionRoot(
            UserAccessStartupConfig config)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(config.Logger));

            var loggerFactory = new SerilogLoggerFactory(config.Logger);
            containerBuilder.RegisterModule(new DataAccessModule(config.ConnectionString, config.DbName, loggerFactory));
            containerBuilder.RegisterModule(new DomainModule());
            containerBuilder.RegisterModule(new JobsModule(config.JobQueue));
            containerBuilder.RegisterModule(new ApplicationModule(config.StorageFactory));
            containerBuilder.RegisterModule(new ProcessingModule(config.InternalQueue));
            containerBuilder.RegisterModule(new EventsBusModule(config.EventBus));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new OutboxModule());
            containerBuilder.RegisterModule(new MapperModule());


            containerBuilder.RegisterInstance(config.ExecutionContextAccessor);
            containerBuilder.RegisterInstance(config.StorageFactory);

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
