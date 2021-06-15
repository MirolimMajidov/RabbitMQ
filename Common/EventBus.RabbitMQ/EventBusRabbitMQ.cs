using RabbitMQ.Client;
using System.Text;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBusRabbitMQ
    {
        public EventBusRabbitMQ(string hostName = "localhost", string exchange = "EventBusRabbitMQ")
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            var connection = factory.CreateConnection();
            Model = connection.CreateModel();

            Exchange = exchange;
        }

        public IModel Model { get; set; }

        public string Exchange { get; set; }

        public void Publish(string message, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);
            Model.BasicPublish(exchange: Exchange, routingKey: routingKey, basicProperties: null, body: body);
        }

        ~EventBusRabbitMQ()
        {
            Model?.Dispose();
            Model = null;
        }
    }
}
