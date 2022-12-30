using Amazon.SQS;
using Serilog;

namespace HelpLine.BuildingBlocks.Bus.SQS;

internal class QueueClientFactory
{
    private readonly ILogger _logger;
    private readonly IAmazonSQS _sqs;
    private readonly ConsumePlanner _planner;
    private readonly byte _collectCount;

    public QueueClientFactory(ILogger logger, IAmazonSQS sqs, ConsumePlanner planner, byte collectCount)
    {
        _logger = logger;
        _sqs = sqs;
        _planner = planner;
        _collectCount = collectCount;
    }

    public QueueClient<THandler> Make<THandler>(string queueUrl,
        Func<SubscriptionHandler<THandler>, string, Task> messageHandler)
    {
        return new QueueClient<THandler>(_sqs, _logger, messageHandler, queueUrl,
            _planner, _collectCount);
    }
}
