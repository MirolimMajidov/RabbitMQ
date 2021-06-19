using EventBus.RabbitMQ;
using System;

namespace SecondMicroservice.EventBusRabbitMQ.Events
{
    public class FirstTestEvent : RabbitMQEvent
    {
        public string Message { get; set; }
    }
}