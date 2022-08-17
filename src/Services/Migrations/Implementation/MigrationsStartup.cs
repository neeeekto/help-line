using System.Threading.Tasks;
using Autofac;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using HelpLine.BuildingBlocks.Services;
using HelpLine.Services.Migrations.Application.Commands.RunAutoMigrations;
using HelpLine.Services.Migrations.Infrastructure;
using MediatR;
using Serilog;

namespace HelpLine.Services.Migrations
{
    public class MigrationsStartup
    {
        private static IContainer _container;

        public static MigrationsStartup Initialize(
            string connectionString,
            string dbName,
            ILogger logger,
            IStorageFactory storageFactory,
            IExecutionContextAccessor contextAccessor,
            IMigrationsRegistry registry
        )
        {
            ConfigureCompositionRoot(
                connectionString, dbName, logger, storageFactory, contextAccessor,  registry);
            return new MigrationsStartup();
        }


        private static void ConfigureCompositionRoot(
            string connectionString,
            string dbName,
            ILogger logger,
            IStorageFactory storageFactory,
            IExecutionContextAccessor contextAccessor,
            IMigrationsRegistry registry
        )
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new DataModule(connectionString, dbName));
            containerBuilder.RegisterModule(new MediatorModule(ServiceInfo.Assembly));
            containerBuilder.RegisterModule(new ProcessingModule(ServiceInfo.Assembly));

            containerBuilder.RegisterInstance(contextAccessor).As<IExecutionContextAccessor>().SingleInstance();
            containerBuilder.RegisterInstance(logger).As<ILogger>().SingleInstance();
            containerBuilder.RegisterInstance(storageFactory.Make("Migrations.State")).SingleInstance();
            containerBuilder.RegisterInstance(registry).SingleInstance();

            _container = containerBuilder.Build();
        }

        internal static ILifetimeScope BeginLifetimeScope()
        {
            return _container.BeginLifetimeScope();
        }


        public async Task Run()
        {
            await using var scope = BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(new RunAutoMigrationsCommand());
        }
    }
}
