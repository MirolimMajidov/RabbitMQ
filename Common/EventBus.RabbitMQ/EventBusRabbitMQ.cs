using Autofac;
using EventBus.RabbitMQ.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBusRabbitMQ
    {
        const string Exchange = "Microservices";

        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IEventBusSubscriptionsManager _subscriptionsManager;
        private readonly IRabbitMQConnection _connection;
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "Microservices";

        private IModel _consumerChannel;
        private string _queueName;

        public EventBusRabbitMQ(IRabbitMQConnection connection, IEventBusSubscriptionsManager subscriptionsManager, ILifetimeScope autofac, ILogger<EventBusRabbitMQ> logger, string queueName)
        {
            _subscriptionsManager = subscriptionsManager;
            _connection = connection;
            _autofac = autofac;
            _logger = logger;
            _queueName = queueName;
            _consumerChannel = CreateConsumerChannel();

            _subscriptionsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        public void Publish(RabbitMQEvent @event)
        {
            OpenRabbitMQConnectionIfItIsNotOpened();

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_connection.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            var eventName = @event.GetType().Name;
            _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: Exchange, type: ExchangeType.Direct);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    channel.BasicPublish(
                        exchange: Exchange,
                        routingKey: eventName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                });
            }
        }

        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : RabbitMQEvent
            where TEventHandler : IRabbitMQEventHandler<TEvent>
        {
            var eventName = _subscriptionsManager.GetEventKey<TEvent>();
            if (!_subscriptionsManager.HasSubscription(eventName))
            {
                OpenRabbitMQConnectionIfItIsNotOpened();
                using var channel = _connection.CreateModel();
                channel.QueueBind(queue: _queueName, exchange: Exchange, routingKey: eventName);
            }

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TEventHandler).GetGenericTypeName());

            _subscriptionsManager.AddSubscription<TEvent, TEventHandler>();

            StartBasicConsume();
        }

        public void Unsubscribe<TEvent, TEventHandler>()
            where TEvent : RabbitMQEvent
            where TEventHandler : IRabbitMQEventHandler<TEvent>
        {
            var eventName = _subscriptionsManager.GetEventKey<TEvent>();
            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subscriptionsManager.RemoveSubscription<TEvent, TEventHandler>();
        }

        private IModel CreateConsumerChannel()
        {
            OpenRabbitMQConnectionIfItIsNotOpened();

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = _connection.CreateModel();

            channel.ExchangeDeclare(exchange: Exchange, type: ExchangeType.Direct);

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private void StartBasicConsume()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call when _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }

                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }

            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_subscriptionsManager.HasSubscription(eventName))
            {
                using var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME);
                var subscription = _subscriptionsManager.GetEventHandler(eventName);
                var handler = scope.ResolveOptional(subscription);
                if (handler == null) return;

                var eventType = _subscriptionsManager.GetEventType(eventName);
                var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                var concreteType = typeof(IRabbitMQEventHandler<>).MakeGenericType(eventType);

                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            OpenRabbitMQConnectionIfItIsNotOpened();

            using var channel = _connection.CreateModel();
            channel.QueueUnbind(queue: _queueName, exchange: Exchange, routingKey: eventName);

            if (_subscriptionsManager.IsEmpty)
            {
                _queueName = string.Empty;
                _consumerChannel.Close();
            }
        }

        void OpenRabbitMQConnectionIfItIsNotOpened()
        {
            if (!_connection.IsConnected)
                _connection.TryConnect();
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            _subscriptionsManager.Clear();
        }
    }
}
