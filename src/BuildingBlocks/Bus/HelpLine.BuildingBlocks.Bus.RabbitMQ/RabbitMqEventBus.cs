using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using Newtonsoft.Json;

namespace HelpLine.BuildingBlocks.Bus.RabbitMQ
{
    internal class RabbitMqEventBus : IEventsBus
    {
        private readonly RabbitMqClient<IIntegrationEventHandler> _client;

        public RabbitMqEventBus(RabbitMqBusClientFactory clientFactory, string queueName)
        {
            _client = clientFactory.Make<IIntegrationEventHandler>(queueName, ProcessEvent);
        }

        private async Task ProcessEvent(
            SubscriptionHandler<IIntegrationEventHandler> subscriptionHandler, string message)
        {
            var @event = JsonConvert.DeserializeObject(message, subscriptionHandler.EventType);
            await (Task) subscriptionHandler.Handler.GetType().GetMethod("Handle")
                ?.Invoke(subscriptionHandler.Handler, new object[] {@event});
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public void Publish<T>(T @event) where T : IntegrationEvent
        {
            _client.Publish(@event);
        }

        public void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
        {
            _client.Subscribe<T>(handler);
        }

        public void StartConsuming()
        {
            _client.StartConsuming();
        }
    }
}
