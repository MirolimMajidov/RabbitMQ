using System;

namespace EventBus.RabbitMQ
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;

        void AddSubscription<TEvent, TEventHandler>()
           where TEvent : RabbitMQEvent
           where TEventHandler : IRabbitMQEventHandler<TEvent>;

        void RemoveSubscription<TEvent, TEventHandler>()
           where TEvent : RabbitMQEvent
           where TEventHandler : IRabbitMQEventHandler<TEvent>;

        bool HasSubscription<TEvent>() where TEvent : RabbitMQEvent;
        bool HasSubscription(string eventName);

        public Type GetEventHandler<TEvent>() where TEvent : RabbitMQEvent;
        public Type GetEventHandler(string eventName);

        public Type GetEventType(string eventName);

        string GetEventKey<T>();
        void Clear();
    }
}