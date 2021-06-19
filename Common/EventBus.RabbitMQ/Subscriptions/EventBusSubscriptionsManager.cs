using System;
using System.Collections.Generic;
using System.Linq;

namespace EventBus.RabbitMQ
{
    public partial class EventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, Type> _handlers;
        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public EventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, Type>();
            _eventTypes = new List<Type>();
        }

        public bool IsEmpty => !_handlers.Keys.Any();

        public void AddSubscription<TEvent, TEventHandler>()
            where TEvent : RabbitMQEvent
            where TEventHandler : IRabbitMQEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);
            var handlerType = typeof(TEventHandler);

            if (HasSubscription(eventType.Name))
                throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventType.Name}'", nameof(handlerType));
            else
                _handlers.Add(eventType.Name, handlerType);

            if (!_eventTypes.Contains(eventType))
                _eventTypes.Add(eventType);
        }

        public void RemoveSubscription<TEvent, TEventHandler>()
            where TEvent : RabbitMQEvent
            where TEventHandler : IRabbitMQEventHandler<TEvent>
        {
            var eventName = GetEventKey<TEvent>();
            if (HasSubscription<TEvent>())
            {
                _handlers.Remove(eventName);
                _eventTypes.Remove(typeof(TEvent));

                RaiseOnEventRemoved(eventName);
            }
        }

        public bool HasSubscription<TEvent>() where TEvent : RabbitMQEvent
        {
            var key = GetEventKey<TEvent>();
            return HasSubscription(key);
        }

        public bool HasSubscription(string eventName) => _handlers.ContainsKey(eventName);


        public Type GetEventHandler<TEvent>() where TEvent : RabbitMQEvent
        {
            if (HasSubscription<TEvent>())
                return GetEventHandler(GetEventKey<TEvent>());

            return null;
        }

        public Type GetEventHandler(string eventName) => _handlers[eventName];

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }

        public Type GetEventType(string eventName) => _eventTypes.FirstOrDefault(t => t.Name == eventName);

        public string GetEventKey<TEvent>()
        {
            return typeof(TEvent).Name;
        }

        public void Clear()
        {
            _handlers.Clear();
            _eventTypes.Clear();
        }
    }
}
