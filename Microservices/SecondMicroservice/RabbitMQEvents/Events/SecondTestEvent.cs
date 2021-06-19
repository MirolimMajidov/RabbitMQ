using EventBus.RabbitMQ;
using System;

namespace SecondMicroservice.RabbitMQEvents.Events
{
    public class SecondTestEvent : RabbitMQEvent
    {
        public string Message { get; set; }
    }
}