using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Serilog;

namespace HelpLine.BuildingBlocks.Bus.RabbitMQ
{
    internal class RabbitMqBusClientFactory
    {
        private readonly IRabbitMqPersistentConnection _connection;
        private readonly string _brokerName;
        private readonly ILogger _logger;

        public RabbitMqBusClientFactory(IConnectionFactory connectionFactory, ILogger logger, string brokerName)
        {
            _logger = logger;
            _brokerName = brokerName;
            _connection = new DefaultRabbitMqPersistentConnection(connectionFactory, logger);
        }

        public RabbitMqClient<THandler> Make<THandler>(string queueName, Func<SubscriptionHandler<THandler>, string, Task> messageHandler, bool useQueueNameAsRoute = false)
        {
            return new RabbitMqClient<THandler>(_connection, _logger, _brokerName, messageHandler, queueName, useQueueNameAsRoute);
        }
    }
}
