using Autofac;
using HelpLine.BuildingBlocks.Bus.EventsBus;

namespace HelpLine.Modules.Quality.Infrastructure.Configuration.EventsBus
{
    internal class EventsBusModule : Autofac.Module
    {
        private readonly IEventsBus _bus;


        public EventsBusModule(IEventsBus bus)
        {
            _bus = bus;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_bus)
                .As<IEventsBus>()
                .SingleInstance();
        }
    }
}
