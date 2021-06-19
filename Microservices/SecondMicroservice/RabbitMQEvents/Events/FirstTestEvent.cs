using EventBus.RabbitMQ;
using System;

namespace SecondMicroservice.RabbitMQEvents.Events
{
    public class FirstTestEvent : RabbitMQEvent
    {
        public string Message { get; set; }
    }
}