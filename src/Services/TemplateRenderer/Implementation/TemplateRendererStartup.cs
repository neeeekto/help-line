using Autofac;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Services;
using HelpLine.Services.TemplateRenderer.Infrastructure;
using Serilog;

namespace HelpLine.Services.TemplateRenderer
{
    public class TemplateRendererStartup
    {
        private static IContainer _container;

        public static TemplateRendererStartup Initialize(
            string connectionString,
            string dbName,
            ILogger logger,
            IExecutionContextAccessor contextAccessor
        )
        {
            ConfigureCompositionRoot(
                connectionString, dbName, logger, contextAccessor);
            return new TemplateRendererStartup();
        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            string dbName,
            ILogger logger,
            IExecutionContextAccessor contextAccessor
        )
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new DataModule(connectionString, dbName));
            containerBuilder.RegisterModule(new MediatorModule(ServiceInfo.Assembly));
            containerBuilder.RegisterModule(new ProcessingModule(ServiceInfo.Assembly));

            containerBuilder.RegisterInstance(contextAccessor).As<IExecutionContextAccessor>().SingleInstance();
            containerBuilder.RegisterInstance(logger).As<ILogger>().SingleInstance();

            _container = containerBuilder.Build();
        }

        internal static ILifetimeScope BeginLifetimeScope()
        {
            return _container.BeginLifetimeScope();
        }
    }
}
