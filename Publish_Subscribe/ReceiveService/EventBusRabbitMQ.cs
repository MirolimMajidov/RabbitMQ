using RabbitMQ.Client;
using System;
using System.Text;

namespace ReceiveService
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

        public void Publish(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            Model.BasicPublish(exchange: Exchange,
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
        }

        ~EventBusRabbitMQ()
        {
            Model?.Dispose();
            Model = null;
        }
    }
}
