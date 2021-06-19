using EventBus.RabbitMQ;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SecondMicroservice.RabbitMQEvents.Events;
using System.Threading.Tasks;

namespace SecondMicroservice.RabbitMQEvents.EventHandlers
{
    public class FirstTestEventHandler : IRabbitMQEventHandler<FirstTestEvent>
    {
        private readonly ILogger<FirstTestEventHandler> _logger;

        public FirstTestEventHandler(ILogger<FirstTestEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(FirstTestEvent @event)
        {
            _logger.LogInformation("Received {Event} event: ({IntegrationEvent})", @event.GetType().Name, JsonConvert.SerializeObject(@event));

            await Task.CompletedTask;
        }
    }
}