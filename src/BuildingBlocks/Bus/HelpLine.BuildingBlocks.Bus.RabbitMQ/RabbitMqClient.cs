using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Serilog;

namespace HelpLine.BuildingBlocks.Bus.RabbitMQ
{
    internal class RabbitMqClient<THandler>
    {
        private readonly IRabbitMqPersistentConnection _persistentConnection;
        private readonly int _retryCount;
        private readonly ILogger _logger;
        private readonly bool _useQueueNameAsRoute;

        private IModel _consumerChannel;
        private readonly string _queueName;
        private readonly string _brokerName;
        private readonly List<SubscriptionHandler<THandler>> _handlers;
        private readonly Func<SubscriptionHandler<THandler>, string, Task> _messageHandler;

        public RabbitMqClient(IRabbitMqPersistentConnection persistentConnection, ILogger logger, string brokerName,
            Func<SubscriptionHandler<THandler>, string, Task> messageHandler, string queueName, bool useQueueNameAsRoute, int retryCount = 5
            )
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _brokerName = brokerName;
            _messageHandler = messageHandler;
            _queueName = queueName;
            _useQueueNameAsRoute = useQueueNameAsRoute;
            _retryCount = retryCount;
            _handlers = new List<SubscriptionHandler<THandler>>();
        }

        public void Dispose()
        {
            _consumerChannel?.Dispose();
        }

        public void Publish<T>(T @event)
        {
            ReconnectIfNeeded();

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) =>
                    {
                        _logger.Warning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})",
                            @event.GetType().FullName, $"{time.TotalSeconds:n1}", ex.Message);
                    });

            var eventName = GetEventName(@event);

            using var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(_brokerName, "direct");
            var message = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            var body = Encoding.UTF8.GetBytes(message);

            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(
                    exchange: _brokerName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            });
        }

        public void Subscribe<T>(THandler handler)
        {
            var eventName = GetEventName<T>();
            DoInternalSubscription(eventName);
            _logger.Debug("Subscribing to event {EventName} with {EventHandler}", eventName, handler.GetType().Name);
            _handlers.Add(new SubscriptionHandler<THandler>(handler, eventName, typeof(T)));
        }

        private string GetEventName<T>()
        {
            return _useQueueNameAsRoute ? _queueName : typeof(T).FullName;
        }

        private string GetEventName<T>(T @event)
        {
            return _useQueueNameAsRoute ? _queueName : @event.GetType().FullName;
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _handlers.Any(h => h.EventName == eventName);
            if (!containsKey)
            {
                ReconnectIfNeeded();
                using var channel = _persistentConnection.CreateModel();
                channel.QueueBind(queue: _queueName,
                    exchange: _brokerName,
                    routingKey: eventName
                );
            }
        }

        public void StartConsuming()
        {
            _consumerChannel = CreateConsumerChannel();
            StartBasicConsume();
        }

        private void StartBasicConsume()
        {
            _logger.Debug("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.Error("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");

                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.Warning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }

            // Even on exception we take the message off the queue.
            // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX).
            // For more information see: https://www.rabbitmq.com/dlx.html
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }


        private IModel CreateConsumerChannel()
        {
            ReconnectIfNeeded();
            _logger.Debug("Creating RabbitMQ consumer channel");
            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(exchange: _brokerName,
                type: "direct");
            channel.QueueDeclare(queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.Warning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                StartConsuming();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.Debug("Processing RabbitMQ event: {EventName}", eventName);

            var handlers = _handlers.Where(x => x.EventName == eventName).ToList();

            if (handlers.Any())
            {
                foreach (var handler in handlers)
                {
                    await _messageHandler(handler, message);
                }
            }
            else
            {
                _logger.Warning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }

        private void ReconnectIfNeeded()
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();
        }
    }
}
