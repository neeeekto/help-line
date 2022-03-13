using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.BuildingBlocks.Bus.Queue;
using RabbitMQ.Client;
using Serilog;

namespace HelpLine.BuildingBlocks.Bus.RabbitMQ
{
    public class RabbitMqServiceFactory : IQueueFactory, IEventBusFactory
    {
        private readonly RabbitMqBusClientFactory _busFactory;

        public RabbitMqServiceFactory(IConnectionFactory connectionFactory, string brokerName, ILogger logger)
        {
            _busFactory = new RabbitMqBusClientFactory(connectionFactory, logger, brokerName);
        }

        public IQueue MakeQueue(string queueName)
        {
            return new RabbitMqQueue(_busFactory, queueName);
        }

        public IEventsBus MakeEventsBus(string queueName)
        {
            return new RabbitMqEventBus(_busFactory, queueName);
        }
    }
}
