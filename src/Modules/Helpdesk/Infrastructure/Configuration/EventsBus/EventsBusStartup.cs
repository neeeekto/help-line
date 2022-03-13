using Autofac;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.Modules.UserAccess.IntegrationEvents;
using Serilog;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.EventsBus
{
    public static class EventsBusStartup
    {
        public static void Initialize(
            ILogger logger)
        {
            SubscribeToIntegrationEvents(logger);
        }

        private static void SubscribeToIntegrationEvents(ILogger logger)
        {
            var eventBus = HelpdeskCompositionRoot.BeginLifetimeScope().Resolve<IEventsBus>();
            eventBus.StartConsuming();

            SubscribeToIntegrationEvent<NewUserCreatedIntegrationEvent>(eventBus, logger);
            SubscribeToIntegrationEvent<UserRemovedIntegrationEvent>(eventBus, logger);
        }

        private static void SubscribeToIntegrationEvent<T>(IEventsBus eventBus, ILogger logger) where T:  IntegrationEvent
        {
            logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
            eventBus.Subscribe(
                new IntegrationEventGenericHandler<T>());
        }
    }
}
