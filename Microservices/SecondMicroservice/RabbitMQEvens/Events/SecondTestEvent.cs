using EventBus.RabbitMQ;
using System;

namespace SecondMicroservice.RabbitMQEvens.Events
{
    public class SecondTestEvent : RabbitMQEvent
    {
        public string Message { get; set; }
    }
}