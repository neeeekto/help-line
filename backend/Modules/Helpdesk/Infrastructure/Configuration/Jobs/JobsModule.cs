using System;
using Autofac;
using Autofac.Features.Decorators;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Configuration.Jobs;
using HelpLine.Services.Jobs.Contracts;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Jobs
{
    internal class JobsModule : Autofac.Module
    {
        private readonly IJobTaskQueue _queue;

        public JobsModule(IJobTaskQueue queue)
        {
            _queue = queue;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_queue)
                .As<IJobTaskQueue>()
                .SingleInstance();


            Func<IDecoratorContext, bool> commandCondition = x =>
            {
                var result = x.ImplementationType.IsClosedTypeOf(typeof(IJobHandler<>));
                return result;
            };

            builder.RegisterGenericDecorator(
                typeof(UnitOfWorkJobHandler<,>),
                typeof(IRequestHandler<,>),
                commandCondition);

            builder.RegisterGenericDecorator(
                typeof(LoggingJobHandlerDecorator<,>),
                typeof(IRequestHandler<,>),
                commandCondition);
        }
    }
}
