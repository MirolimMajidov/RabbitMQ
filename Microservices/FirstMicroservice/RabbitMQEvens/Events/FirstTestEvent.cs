using EventBus.RabbitMQ;

namespace FirstMicroservice.RabbitMQEvens.Events
{
    public class FirstTestEvent : RabbitMQEvent
    {
        public string Message { get; set; }
    }
}