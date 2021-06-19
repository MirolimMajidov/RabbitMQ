using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public interface IRabbitMQEventHandler<in TRabbitMQEvent> : IRabbitMQEventHandler
        where TRabbitMQEvent : RabbitMQEvent
    {
        Task Handle(TRabbitMQEvent @event);
    }

    public interface IRabbitMQEventHandler
    {
    }
}
