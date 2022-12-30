using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Quartz;
using HelpLine.BuildingBlocks.Application;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.BuildingBlocks.Services;
using HelpLine.Services.Jobs.Application;
using HelpLine.Services.Jobs.Application.Commands;
using HelpLine.Services.Jobs.Application.Commands.RunJobs;
using HelpLine.Services.Jobs.Application.Commands.StopJobs;
using HelpLine.Services.Jobs.Application.Contracts;
using HelpLine.Services.Jobs.Infrastructure;
using HelpLine.Services.Jobs.QuartzJobs;
using MediatR;
using Newtonsoft.Json;
using Quartz;
using Serilog;

namespace HelpLine.Services.Jobs
{
    public class JobsStartup
    {
        private static IContainer _container;

        public static JobsStartup Initialize(
            JobsStartupConfig config
        )
        {
            ConfigureCompositionRoot(config);
            return new JobsStartup();
        }

        private static void ConfigureCompositionRoot(
            JobsStartupConfig config
        )
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new DataModule(config.ConnectionString, config.DbName));
            containerBuilder.RegisterModule(new MediatorModule(ServiceInfo.Assembly));
            containerBuilder.RegisterModule(new ProcessingModule(ServiceInfo.Assembly));
            containerBuilder.RegisterModule(new QuartzAutofacFactoryModule());
            containerBuilder.RegisterModule(new QuartzAutofacJobsModule(ServiceInfo.Assembly));

            containerBuilder.RegisterInstance(config.Logger).As<ILogger>().SingleInstance();
            containerBuilder.RegisterInstance(config.ContextAccessor).As<IExecutionContextAccessor>().SingleInstance();
            containerBuilder.RegisterInstance(new JobTasksCollection(config.JobTasksAssemblie)).SingleInstance();
            containerBuilder.RegisterType<JobTaskRunner>()
                .WithParameter("queue", config.Queue)
                .As<IJobTaskRunner>()
                .SingleInstance();
            containerBuilder.RegisterInstance(config.Logger).As<ILogger>().SingleInstance();

            _container = containerBuilder.Build();
        }

        internal static ILifetimeScope BeginLifetimeScope()
        {
            return _container.BeginLifetimeScope();
        }

        public async Task Start()
        {
            await using var scope = BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(new RunJobsCommand());
        }

        public async Task Stop()
        {
            await using var scope = BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            await mediator.Send(new StopJobsCommand());
        }
    }
}
