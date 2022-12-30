using Autofac;
using HelpLine.BuildingBlocks.Bus.EventsBus;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.EventsBus
{
    internal class EventsBusModule : Autofac.Module
    {
        private readonly IEventsBus _eventsBux;

        public EventsBusModule(IEventsBus eventsBux)
        {
            _eventsBux = eventsBux;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_eventsBux)
                .As<IEventsBus>()
                .SingleInstance();
        }
    }
}
