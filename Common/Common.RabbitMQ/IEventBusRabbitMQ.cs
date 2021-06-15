using RabbitMQ.Client;

namespace Common.RabbitMQ
{
    public interface IEventBusRabbitMQ
    {
        IModel Model { get; set; }

        string Exchange { get; set; }

        void Publish(string message, string routingKey, IBasicProperties basicProperties = null);
    }
}
