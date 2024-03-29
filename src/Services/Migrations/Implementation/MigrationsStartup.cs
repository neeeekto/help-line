﻿using System.Threading.Tasks;
using Autofac;
using HelpLine.BuildingBlocks.Application;
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

        public static MigrationsStartup Initialize(MigrationsStartupConfig config)
        {
            ConfigureCompositionRoot(config);
            return new MigrationsStartup();
        }


        private static void ConfigureCompositionRoot(MigrationsStartupConfig config)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new DataModule(config.ConnectionString, config.DbName));
            containerBuilder.RegisterModule(new MediatorModule(ServiceInfo.Assembly));
            containerBuilder.RegisterModule(new ProcessingModule(ServiceInfo.Assembly));

            containerBuilder.RegisterInstance(config.ContextAccessor).As<IExecutionContextAccessor>().SingleInstance();
            containerBuilder.RegisterInstance(config.Logger).As<ILogger>().SingleInstance();
            containerBuilder.RegisterInstance(config.StorageFactory.Make("Migrations.State")).SingleInstance();
            containerBuilder.RegisterInstance(config.Registry).SingleInstance();

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
