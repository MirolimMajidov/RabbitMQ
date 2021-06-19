using EventBus.RabbitMQ;

namespace FirstMicroservice.RabbitMQEvents.Events
{
    public class SecondTestEvent : RabbitMQEvent
    {
        public string Message { get; set; }
    }
}