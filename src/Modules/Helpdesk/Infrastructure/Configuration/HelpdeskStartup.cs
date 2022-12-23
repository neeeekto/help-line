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
            string connectionString,
            string dbName,
            IQueueFactory queueFactory,
            IEventBusFactory busFactory,
            IJobTaskQueue jobTaskQueue,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            ITemplateRenderer templateRenderer,
            IEmailSender emailSender,
            IMigrationCollector? migrationCollector = null
        )
        {
            ConfigureCompositionRoot(
                connectionString,
                dbName,
                queueFactory,
                busFactory,
                jobTaskQueue,
                executionContextAccessor,
                logger,
                templateRenderer,
                emailSender,
                migrationCollector);

            return new HelpdeskStartup(logger);
        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            string dbName,
            IQueueFactory queueFactory,
            IEventBusFactory busFactory,
            IJobTaskQueue jobTaskQueue,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            ITemplateRenderer templateRenderer,
            IEmailSender emailSender,
            IMigrationCollector? migrationCollector = null
        )
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "Helpdesk")));

            var loggerFactory = new SerilogLoggerFactory(logger);
            containerBuilder.RegisterModule(new DataAccessModule(connectionString, dbName, loggerFactory));
            containerBuilder.RegisterModule(new DomainModule());
            containerBuilder.RegisterModule(new ProcessingModule(queueFactory));
            containerBuilder.RegisterModule(new EventsBusModule(busFactory));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new OutboxModule());
            containerBuilder.RegisterModule(new ApplicationModule());
            containerBuilder.RegisterModule(new MapperModule());
            containerBuilder.RegisterModule(new JobsModule(jobTaskQueue));

            containerBuilder.RegisterInstance(executionContextAccessor);
            containerBuilder.RegisterInstance(templateRenderer);
            containerBuilder.RegisterInstance(emailSender);

            _container = containerBuilder.Build();
            HelpdeskCompositionRoot.SetContainer(_container);
            if (migrationCollector != null)
            {
                var migrations = _container.Resolve<IEnumerable<IMigrationInstance>>();
                foreach (var migration in migrations)
                {
                    migrationCollector.Add(() => migration);
                }
            }
        }

        private readonly ILogger _moduleLogger;

        internal HelpdeskStartup(ILogger moduleLogger)
        {
            _moduleLogger = moduleLogger;
        }

        public HelpdeskStartup EnableAppQueueHandling()
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
