using EventBus.RabbitMQ;
using System;

namespace SecondMicroservice.EventBusRabbitMQ.Events
{
    public class SecondTestEvent : RabbitMQEvent
    {
        public string Message { get; set; }
    }
}