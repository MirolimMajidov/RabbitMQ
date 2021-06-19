using EventBus.RabbitMQ;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SecondMicroservice.EventBusRabbitMQ.Events;
using System.Threading.Tasks;

namespace SecondMicroservice.EventBusRabbitMQ.EventHandlers
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
            _logger.LogInformation("Received {Event} event on {AppName} app: ({IntegrationEvent})", @event.GetType().Name, "SecondMicroservice", JsonConvert.SerializeObject(@event));

            await Task.CompletedTask;
        }
    }
}