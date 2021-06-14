using RabbitMQ.Client;
using System;
using System.Text;

namespace SenderService
{
    public class EventBusRabbitMQ : IEventBusRabbitMQ
    {
        public EventBusRabbitMQ()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            Model = connection.CreateModel();
        }

        public IModel Model { get; set; }

        public string Exchange { get; set; }

        public void Publish(string message, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);
            Model.BasicPublish(exchange: Exchange,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
        }

        ~EventBusRabbitMQ()
        {
            Model?.Dispose();
            Model = null;
        }
    }
}
