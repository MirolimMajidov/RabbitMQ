using EventBus.RabbitMQ;
using System;

namespace SecondMicroservice.RabbitMQEvens.Events
{
    public class FirstTestEvent : RabbitMQEvent
    {
        public string Message { get; set; }
    }
}