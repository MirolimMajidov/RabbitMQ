using RabbitMQ.Client;
using System.Text;

namespace Common.RabbitMQ
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
