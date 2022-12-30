using System;
using Autofac;
using Autofac.Features.Decorators;
using HelpLine.BuildingBlocks.Application.Events;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure;
using HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using HelpLine.BuildingBlocks.Infrastructure.InternalCommands;
using HelpLine.Modules.UserAccess.Application.Configuration.Commands;
using HelpLine.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands;
using MediatR;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    internal class ProcessingModule : Autofac.Module
    {
        private readonly IQueue _queue;


        public ProcessingModule(IQueue queue)
        {
            _queue = queue;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DomainEventsDispatcher>()
                .As<IDomainEventsDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventsAccessorAndCollector>()
                .As<IDomainEventsAccessor>()
                .As<IDomainEventCollector>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CommandsScheduler>()
                .As<ICommandsScheduler>()
                .InstancePerLifetimeScope();

            builder.RegisterType<InternalCommandsQueue<InternalCommandTask>>()
                .WithParameter("queue", _queue)
                .As<IInternalCommandsQueue>()
                .InstancePerLifetimeScope();

            builder.RegisterType<InternalCommandTaskRepository>()
                .As<IInternalCommandTaskRepository>()
                .InstancePerLifetimeScope();

            //
            Func<IDecoratorContext, bool> commandCondition = x =>
                x.ImplementationType.IsClosedTypeOf(typeof(ICommandHandler<>)) ||
                x.ImplementationType.IsClosedTypeOf(typeof(ICommandHandler<,>));

            builder.RegisterGenericDecorator(
                typeof(UnitOfWorkCommandHandlerDecorator<,>),
                typeof(IRequestHandler<,>),
                commandCondition);

            builder.RegisterGenericDecorator(
                typeof(ValidationCommandHandlerDecorator<,>),
                typeof(IRequestHandler<,>),
                commandCondition);

            builder.RegisterGenericDecorator(
                typeof(LoggingCommandHandlerDecorator<,>),
                typeof(IRequestHandler<,>),
                commandCondition);



            builder.RegisterGenericDecorator(
                typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
                typeof(INotificationHandler<>));

            builder.RegisterAssemblyTypes(Assemblies.Application)
                .AsClosedTypesOf(typeof(IDomainEventNotification<>))
                .InstancePerDependency()
                .FindConstructorsWith(new AllConstructorFinder());
        }
    }
}
