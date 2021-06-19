using EventBus.RabbitMQ;
using System;

namespace FirstMicroservice.EventBusRabbitMQ.Events
{
    public class SecondTestEvent : RabbitMQEvent
    {
        public string Message { get; set; }
    }
}