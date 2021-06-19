using Newtonsoft.Json;
using System;

namespace EventBus.RabbitMQ
{
    public abstract class RabbitMQEvent
    {
        public RabbitMQEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public RabbitMQEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }
    }
}
