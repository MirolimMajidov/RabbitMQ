using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBusRabbitMQ
    {
        private readonly ILogger<EventBusRabbitMQ> _logger;

        public IModel Model { get; set; }

        public string Exchange { get; set; }

        public EventBusRabbitMQ(ILogger<EventBusRabbitMQ> logger, string hostName, string exchange = "Microservices")
        {
            _logger = logger;
            var factory = new ConnectionFactory() { HostName = hostName };
            var connection = factory.CreateConnection();
            Model = connection.CreateModel();
            Exchange = exchange;
            Model.ExchangeDeclare(exchange: Exchange, type: ExchangeType.Direct);
        }

        public void Publish(string message, string routingKey, IBasicProperties basicProperties = null)
        {
            var body = Encoding.UTF8.GetBytes(message);
            Model.BasicPublish(exchange: Exchange, routingKey: routingKey, basicProperties: basicProperties, body: body);
        }

        ~EventBusRabbitMQ()
        {
            Model?.Dispose();
            Model = null;
        }
    }
}
