using System;
using Autofac;
using Autofac.Features.Decorators;
using HelpLine.Modules.UserAccess.Application.Configuration.Jobs;
using HelpLine.Services.Jobs.Contracts;
using MediatR;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration.Jobs
{
    internal class JobsModule : Autofac.Module
    {
        private readonly IJobTaskQueue _queueFactory;

        public JobsModule(IJobTaskQueue queueFactory)
        {
            _queueFactory = queueFactory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_queueFactory)
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
