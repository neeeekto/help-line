using Autofac;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.Modules.Helpdesk.IntegrationEvents;
using Serilog;

namespace HelpLine.Modules.Quality.Infrastructure.Configuration.EventsBus
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
            var eventBus = QualityCompositionRoot.BeginLifetimeScope().Resolve<IEventsBus>();
            eventBus.StartConsuming();
            SubscribeToIntegrationEvent<TicketClosedIntegrationEvent>(eventBus, logger);
            SubscribeToIntegrationEvent<TicketCreatedIntegrationEvent>(eventBus, logger);
            SubscribeToIntegrationEvent<TicketFeedbackAddedIntegrationEvent>(eventBus, logger);
            SubscribeToIntegrationEvent<TicketIncomingMessageAddedIntegrationEvent>(eventBus, logger);
            SubscribeToIntegrationEvent<TicketOutgoingMessageAddedIntegrationEvent>(eventBus, logger);
            SubscribeToIntegrationEvent<TicketNotePostedIntegrationEvent>(eventBus, logger);
            SubscribeToIntegrationEvent<TicketNoteRemovedIntegrationEvent>(eventBus, logger);
            SubscribeToIntegrationEvent<TicketStatusChangedIntegrationEvent>(eventBus, logger);
        }

        private static void SubscribeToIntegrationEvent<T>(IEventsBus eventBus, ILogger logger) where T:  IntegrationEvent
        {
            logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
            eventBus.Subscribe(
                new IntegrationEventGenericHandler<T>());
        }
    }
}
