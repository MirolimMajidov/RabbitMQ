using EventBus.RabbitMQ;
using SecondMicroservice.RabbitMQEvents.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SecondMicroservice.RabbitMQEvents.EventHandlers
{
    public class SecondTestEventHandler : IRabbitMQEventHandler<SecondTestEvent>
    {
        private readonly ILogger<SecondTestEventHandler> _logger;

        public SecondTestEventHandler(ILogger<SecondTestEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(SecondTestEvent @event)
        {
            _logger.LogInformation("Received {Event} event: ({IntegrationEvent})", @event.GetType().Name, JsonConvert.SerializeObject(@event));

            await Task.CompletedTask;
        }
    }
}