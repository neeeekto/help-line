using HelpLine.BuildingBlocks.Bus.EventsBus;
using Newtonsoft.Json;

namespace HelpLine.BuildingBlocks.Bus.SQS;

internal class SqsBus : IEventsBus
{
    private readonly QueueClient<IIntegrationEventHandler> _queueClient;
    public SqsBus(QueueClientFactory factory, string queueUrl)
    {
        _queueClient = factory.Make<IIntegrationEventHandler>(queueUrl, ProcessEvent);
    }
    
    private static async Task ProcessEvent(
        SubscriptionHandler<IIntegrationEventHandler> subscriptionHandler, string message)
    {
        var @event = JsonConvert.DeserializeObject(message, subscriptionHandler.EventType);
        await (Task) subscriptionHandler.Handler.GetType().GetMethod("Handle")
            ?.Invoke(subscriptionHandler.Handler, new object[] {@event});
    }
    
    public void Dispose()
    {
        _queueClient.Dispose();
    }
    

    public void StartConsuming()
    {
        _queueClient.StartConsuming();
    }

    public Task Publish<T>(T evt) where T : IntegrationEvent
    {
        return _queueClient.Publish(evt);
    }

    public void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
    {
        _queueClient.Subscribe<T>(handler);
    }
}
