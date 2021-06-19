namespace EventBus.RabbitMQ
{
    public interface IEventBusRabbitMQ
    {
        void Publish(RabbitMQEvent @event);

        void Subscribe<TEvent, TEventHandler>()
            where TEvent : RabbitMQEvent
            where TEventHandler : IRabbitMQEventHandler<TEvent>;

        void Unsubscribe<TEvent, TEventHandler>()
            where TEvent : RabbitMQEvent
            where TEventHandler : IRabbitMQEventHandler<TEvent>;
    }
}
