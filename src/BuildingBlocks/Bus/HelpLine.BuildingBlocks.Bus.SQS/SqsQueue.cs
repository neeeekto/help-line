using System.Text.Json;
using HelpLine.BuildingBlocks.Bus.Queue;

namespace HelpLine.BuildingBlocks.Bus.SQS;

internal class SqsQueue : IQueue
{
    private readonly QueueClient<IQueueHandler> _queueClient;
    public SqsQueue(QueueClientFactory factory, string queueUrl)
    {
        _queueClient = factory.Make<IQueueHandler>(queueUrl, ProcessEvent);
    }
    
    private static async Task ProcessEvent(SubscriptionHandler<IQueueHandler> subscriptionHandler, string message)
    {
        var @event = JsonSerializer.Deserialize(message, subscriptionHandler.EventType);
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

    public Task Add<T>(T message, byte priority = 0)
    {
        return _queueClient.Publish(message);
    }

    public void AddHandler<T>(IQueueHandler<T> handler)
    {
        _queueClient.Subscribe<T>(handler);
    }
}
