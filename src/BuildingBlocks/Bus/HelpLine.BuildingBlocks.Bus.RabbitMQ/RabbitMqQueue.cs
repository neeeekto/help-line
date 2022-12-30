using System.Text.Json;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Bus.Queue;

namespace HelpLine.BuildingBlocks.Bus.RabbitMQ
{
    internal class RabbitMqQueue : IQueue
    {
        private readonly RabbitMqClient<IQueueHandler> _client;

        public RabbitMqQueue(RabbitMqBusClientFactory clientFactory, string queueName)
        {
            _client = clientFactory.Make<IQueueHandler>(queueName, ProcessEvent, true);
        }

        private async Task ProcessEvent(SubscriptionHandler<IQueueHandler> subscriptionHandler, string message)
        {
            var @event = JsonSerializer.Deserialize(message, subscriptionHandler.EventType);
            await (Task) subscriptionHandler.Handler.GetType().GetMethod("Handle")
                ?.Invoke(subscriptionHandler.Handler, new object[] {@event});
        }


        public void StartConsuming()
        {
            _client.StartConsuming();
        }

        public async Task Add<T>(T message, byte priority)
        {
            _client.Publish(message, priority);
        }

        public void AddHandler<T>(IQueueHandler<T> handler)
        {
            _client.Subscribe<T>(handler);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
