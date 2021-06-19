using EventBus.RabbitMQ;

namespace FirstMicroservice.RabbitMQEvents.Events
{
    public class FirstTestEvent : RabbitMQEvent
    {
        public string Message { get; set; }
    }
}