using EventBus.RabbitMQ;

namespace FirstMicroservice.EventBusRabbitMQ.Events
{
    public class FirstTestEvent : RabbitMQEvent
    {
        public string Message { get; set; }
    }
}