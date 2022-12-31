using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using Serilog;

namespace HelpLine.BuildingBlocks.Bus.SQS;

internal class QueueClient<THandler>
{
    private const string KEY = "key";

    private readonly IAmazonSQS _sqs;
    private readonly int _collectCount;
    private readonly int _retryCount;
    private readonly ILogger _logger;

    private readonly string _queueUrl;
    private readonly List<SubscriptionHandler<THandler>> _handlers;
    private readonly Func<SubscriptionHandler<THandler>, string, Task> _messageHandler;
    private readonly ConsumePlanner _planner;

    public QueueClient(IAmazonSQS sqs, ILogger logger,
        Func<SubscriptionHandler<THandler>, string, Task> messageHandler, string queueUrl, ConsumePlanner planner,
        int collectCount, int retryCount = 5
    )
    {
        _sqs = sqs;
        _logger = logger;
        _messageHandler = messageHandler;
        _queueUrl = queueUrl;
        _planner = planner;
        _collectCount = collectCount;
        _retryCount = retryCount;
        _handlers = new List<SubscriptionHandler<THandler>>();
    }

    public void Dispose()
    {
        _planner.Delete(Consume);
    }

    public async Task Publish<T>(T @event)
    {
        _logger.Debug("Publish message in SQS {type}", typeof(T).FullName);
        var message = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        await _sqs.SendMessageAsync(new SendMessageRequest()
        {
            QueueUrl = _queueUrl,
            MessageAttributes = new Dictionary<string, MessageAttributeValue>()
            {
                {
                    KEY, new MessageAttributeValue()
                    {
                        StringValue = typeof(T).FullName,
                        DataType = "String"
                    }
                }
            },
            MessageBody = message,
        });
    }

    public void Subscribe<T>(THandler handler)
    {
        var eventName = typeof(T).FullName!;
        _logger.Debug("Subscribing to event {EventName} with {EventHandler}", eventName, handler.GetType().Name);
        _handlers.Add(new SubscriptionHandler<THandler>(handler, eventName, typeof(T)));
    }


    public void StartConsuming()
    {
        _planner.Add(Consume);
        _planner.Start();
    }

    private async Task Consume()
    {
        _logger.Debug("Consume SQS queue from {queueUrl}", _queueUrl);
        var response = await _sqs.ReceiveMessageAsync(new ReceiveMessageRequest()
        {
            QueueUrl = _queueUrl,
            MaxNumberOfMessages = _collectCount,
            WaitTimeSeconds = 0,
            
        });

        if (response is not null && response.Messages.Count > 0)
        {
            _logger.Debug("Handle SQS messages from {queueUrl}, count: {count}", _queueUrl, response.Messages.Count);
            foreach (var msg in response.Messages)
            {
                var eventName = msg.MessageAttributes[KEY]?.StringValue;
                if (eventName is null)
                {
                    _logger.Debug($"Can't handle message with unknown type, message: {msg.Body}");
                    continue;
                }
                _logger.Debug("Processing SQS message: {eventName}", eventName);
                var handlers = _handlers.Where(x => x.EventName == eventName).ToList();
                if (handlers.Any())
                {
                    foreach (var handler in handlers)
                    {
                        await _messageHandler(handler, msg.Body);
                    }
                    _logger.Debug("SQS message {eventName} handled", eventName);
                }
                else
                {
                    _logger.Warning("No subscription for SQS message: {EventName}", eventName);
                }

                await _sqs.DeleteMessageAsync(new DeleteMessageRequest()
                {
                    QueueUrl = _queueUrl,
                    ReceiptHandle = msg.ReceiptHandle
                });

            }
        }
    }
}
