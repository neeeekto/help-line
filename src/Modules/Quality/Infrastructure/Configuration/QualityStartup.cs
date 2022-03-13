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
            string connectionString,
            string dbName,
            IQueueFactory queueFactory,
            IEventBusFactory busFactory,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger)
        {
            ConfigureCompositionRoot(connectionString,
                dbName,
                queueFactory,
                busFactory,
                executionContextAccessor,
                logger);

            return new QualityStartup(logger);

        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            string dbName,
            IQueueFactory queueFactory,
            IEventBusFactory busFactory,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(logger));

            var loggerFactory = new SerilogLoggerFactory(logger);
            containerBuilder.RegisterModule(new DataAccessModule(connectionString, dbName, loggerFactory));
            containerBuilder.RegisterModule(new DomainModule());
            containerBuilder.RegisterModule(new ProcessingModule(queueFactory));
            containerBuilder.RegisterModule(new EventsBusModule(busFactory));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new OutboxModule());
            containerBuilder.RegisterModule(new MapperModule());


            containerBuilder.RegisterInstance(executionContextAccessor);

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
