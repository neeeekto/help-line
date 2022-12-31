using System.Collections.Generic;
using Autofac;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Application.Emails;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Application;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.DataAccess;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Domain;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.EventsBus;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Jobs;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Logging;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Mapping;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Mediation;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing.Outbox;
using HelpLine.Services.Jobs.Contracts;
using HelpLine.Services.Migrations.Contracts;
using HelpLine.Services.TemplateRenderer.Contracts;
using Serilog;
using Serilog.AspNetCore;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration
{
    public class HelpdeskStartup
    {
        private static IContainer _container;

        public static HelpdeskStartup Initialize(
            HelpdeskStartupConfig config
        )
        {
            ConfigureCompositionRoot(
                config);

            return new HelpdeskStartup(config.Logger);
        }

        private static void ConfigureCompositionRoot(
            HelpdeskStartupConfig config
        )
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(config.Logger.ForContext("Module", "Helpdesk")));

            var loggerFactory = new SerilogLoggerFactory(config.Logger);
            containerBuilder.RegisterModule(new DataAccessModule(config.ConnectionString, config.DbName, loggerFactory));
            containerBuilder.RegisterModule(new DomainModule());
            containerBuilder.RegisterModule(new ProcessingModule(config.InternalQueue));
            containerBuilder.RegisterModule(new EventsBusModule(config.EventBus));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new OutboxModule());
            containerBuilder.RegisterModule(new ApplicationModule());
            containerBuilder.RegisterModule(new MapperModule());
            containerBuilder.RegisterModule(new JobsModule(config.JobQueue));

            containerBuilder.RegisterInstance(config.ExecutionContextAccessor);
            containerBuilder.RegisterInstance(config.TemplateRenderer);
            containerBuilder.RegisterInstance(config.EmailSender);

            _container = containerBuilder.Build();
            HelpdeskCompositionRoot.SetContainer(_container);
            if (config.MigrationCollector != null)
            {
                var migrations = _container.Resolve<IEnumerable<IMigrationInstance>>();
                foreach (var migration in migrations)
                {
                    config.MigrationCollector.Add(() => migration);
                }
            }
        }

        private readonly ILogger _moduleLogger;

        internal HelpdeskStartup(ILogger moduleLogger)
        {
            _moduleLogger = moduleLogger;
        }

        public HelpdeskStartup EnableQueueAndBusHandling()
        {
            EventsBusStartup.Initialize(_moduleLogger);
            ProcessingStartup.Initialize(_moduleLogger);
            return this;
        }

        public HelpdeskStartup EnableJobHandling()
        {
            JobsStartup.Initialize(_moduleLogger);
            return this;
        }
    }
}
