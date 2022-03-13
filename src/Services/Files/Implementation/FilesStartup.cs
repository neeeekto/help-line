using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Autofac;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Services;
using HelpLine.Services.Files.Infrastructure;
using HelpLine.Services.Files.Models;
using Serilog;

namespace HelpLine.Services.Files
{
    public class FilesStartup
    {
        private static IContainer _container;

        public static FilesStartup Initialize(
            ILogger logger,
            IExecutionContextAccessor contextAccessor,
            AwsSettings awsSettings
        )
        {
            ConfigureCompositionRoot(logger, contextAccessor, awsSettings);
            return new FilesStartup();
        }

        private static void ConfigureCompositionRoot(
            ILogger logger,
            IExecutionContextAccessor contextAccessor,
            AwsSettings awsSettings
        )
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new MediatorModule(ServiceInfo.Assembly));
            containerBuilder.RegisterModule(new ProcessingModule(ServiceInfo.Assembly));

            containerBuilder.RegisterInstance(contextAccessor).As<IExecutionContextAccessor>().SingleInstance();
            containerBuilder.RegisterInstance(logger).As<ILogger>().SingleInstance();
            containerBuilder.RegisterInstance(awsSettings).As<AwsSettings>().SingleInstance();

            _container = containerBuilder.Build();
        }

        internal static ILifetimeScope BeginLifetimeScope()
        {
            return _container.BeginLifetimeScope();
        }
    }
}
