using System;
using Autofac;
using Autofac.Features.Decorators;
using HelpLine.BuildingBlocks.Application.Events;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure;
using HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using HelpLine.BuildingBlocks.Infrastructure.InternalCommands;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing.InternalCommands;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing
{
    internal class ProcessingModule : Autofac.Module
    {
        private readonly IQueue _internalQueue;


        public ProcessingModule(IQueue internalQueue)
        {
            _internalQueue = internalQueue;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DomainEventsDispatcher>()
                .As<IDomainEventsDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CommandsScheduler>()
                .As<ICommandsScheduler>()
                .InstancePerLifetimeScope();

            builder.RegisterType<InternalCommandsQueue<InternalCommandTask>>()
                .WithParameter("queue", _internalQueue)
                .As<IInternalCommandsQueue>()
                .InstancePerLifetimeScope();

            // Last
            Func<IDecoratorContext, bool> commandCondition = x =>
                x.ImplementationType.IsClosedTypeOf(typeof(ICommandHandler<>)) ||
                x.ImplementationType.IsClosedTypeOf(typeof(ICommandHandler<,>));


            builder.RegisterGenericDecorator(
                typeof(ProjectionRunnerCommandHandlerDecorator<,>),
                typeof(IRequestHandler<,>),
                commandCondition);

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
                commandCondition
            );

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
