using Amazon.SQS;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.BuildingBlocks.Bus.Queue;
using Serilog;

namespace HelpLine.BuildingBlocks.Bus.SQS;

public class SqsServiceFactory : IQueueFactory, IEventBusFactory
{
    private readonly IAmazonSQS _sqsClient;
    private readonly QueueClientFactory _clientFactory;

    public SqsServiceFactory(IAmazonSQS sqsClient, ILogger logger, TimeSpan interval, byte collectCount)
    {
        _sqsClient = sqsClient;
        var planner = new ConsumePlanner(interval, logger);
        _clientFactory = new QueueClientFactory(logger, sqsClient, planner, collectCount);
    }

    public IQueue MakeQueue(string queueUrl)
    {
        return new SqsQueue(_clientFactory, queueUrl);
    }

    public IEventsBus MakeEventsBus(string queueUrl)
    {
        return new SqsBus(_clientFactory, queueUrl);
    }
}
