using EventBus.RabbitMQ;
using System;

namespace FirstMicroservice.RabbitMQEvens.Events
{
    public class SecondTestEvent : RabbitMQEvent
    {
        public string Message { get; set; }
    }
}