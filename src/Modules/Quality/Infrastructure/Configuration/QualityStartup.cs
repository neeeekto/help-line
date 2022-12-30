using Autofac;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.Modules.Quality.Infrastructure.Configuration.DataAccess;
using HelpLine.Modules.Quality.Infrastructure.Configuration.Domain;
using HelpLine.Modules.Quality.Infrastructure.Configuration.EventsBus;
using HelpLine.Modules.Quality.Infrastructure.Configuration.Logging;
using HelpLine.Modules.Quality.Infrastructure.Configuration.Mapper;
using HelpLine.Modules.Quality.Infrastructure.Configuration.Mediation;
using HelpLine.Modules.Quality.Infrastructure.Configuration.Processing;
using HelpLine.Modules.Quality.Infrastructure.Configuration.Processing.Outbox;
using Serilog;
using Serilog.AspNetCore;

namespace HelpLine.Modules.Quality.Infrastructure.Configuration
{
    public class QualityStartup
    {
        private static IContainer _container;

        public static QualityStartup Initialize(
            QualityStartupConfig config)
        {
            ConfigureCompositionRoot(config);

            return new QualityStartup(config.Logger);

        }

        private static void ConfigureCompositionRoot(
            QualityStartupConfig config)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(config.Logger));

            var loggerFactory = new SerilogLoggerFactory(config.Logger);
            containerBuilder.RegisterModule(new DataAccessModule(config.ConnectionString, config.DbName, loggerFactory));
            containerBuilder.RegisterModule(new DomainModule());
            containerBuilder.RegisterModule(new ProcessingModule(config.InternalQueue));
            containerBuilder.RegisterModule(new EventsBusModule(config.EventBus));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new OutboxModule());
            containerBuilder.RegisterModule(new MapperModule());


            containerBuilder.RegisterInstance(config.ExecutionContextAccessor);

            _container = containerBuilder.Build();

            QualityCompositionRoot.SetContainer(_container);
        }

        private readonly ILogger _moduleLogger;

        internal QualityStartup(ILogger moduleLogger)
        {
            _moduleLogger = moduleLogger;
        }

        public QualityStartup EnableAppQueueHandling()
        {
            EventsBusStartup.Initialize(_moduleLogger);
            ProcessingStartup.Initialize(_moduleLogger);
            return this;
        }

        public QualityStartup EnableJobHandling()
        {
            return this;
        }
    }
}
