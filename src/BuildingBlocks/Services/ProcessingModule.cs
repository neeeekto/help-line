using System.Reflection;
using Autofac;
using HelpLine.BuildingBlocks.Services.Decorators;
using MediatR;

namespace HelpLine.BuildingBlocks.Services
{
    public class ProcessingModule : Autofac.Module
    {
        private readonly Assembly _targetAssembly;

        public ProcessingModule(Assembly targetAssembly)
        {
            _targetAssembly = targetAssembly;
        }

        protected override void Load(ContainerBuilder builder)
        {

            // Last

            builder.RegisterGenericDecorator(
                typeof(ValidationCommandHandlerDecorator<,>),
                typeof(IRequestHandler<,>));


            builder.RegisterGenericDecorator(
                typeof(LoggingHandlerDecorator<,>),
                typeof(IRequestHandler<,>));
        }
    }
}
