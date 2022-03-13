using Autofac;
using HelpLine.BuildingBlocks.Application.Outbox;
using HelpLine.Modules.UserAccess.Infrastructure.Outbox;
using Module = Autofac.Module;

namespace HelpLine.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox
{
    internal class OutboxModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OutboxAccessor>()
                .As<IOutbox>()
                .FindConstructorsWith(new AllConstructorFinder())
                .InstancePerLifetimeScope();
        }
    }
}