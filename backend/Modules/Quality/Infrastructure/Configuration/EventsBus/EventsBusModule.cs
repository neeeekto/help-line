using Autofac;
using HelpLine.BuildingBlocks.Bus.EventsBus;

namespace HelpLine.Modules.Quality.Infrastructure.Configuration.EventsBus
{
    internal class EventsBusModule : Autofac.Module
    {
        private readonly IEventBusFactory _busFactory;

        public EventsBusModule(IEventBusFactory busFactory)
        {
            _busFactory = busFactory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_busFactory.MakeEventsBus("HelpLine.Quality.EventBus"))
                .As<IEventsBus>()
                .SingleInstance();
        }
    }
}
