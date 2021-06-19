using EventBus.RabbitMQ;
using SecondMicroservice.EventBusRabbitMQ.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SecondMicroservice.EventBusRabbitMQ.EventHandlers
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
            _logger.LogInformation("Received {Event} event on {AppName} app: ({IntegrationEvent})", @event.GetType().Name, "SecondMicroservice", JsonConvert.SerializeObject(@event));

            await Task.CompletedTask;
        }
    }
}