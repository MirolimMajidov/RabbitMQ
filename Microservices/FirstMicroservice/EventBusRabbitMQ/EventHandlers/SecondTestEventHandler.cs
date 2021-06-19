using EventBus.RabbitMQ;
using FirstMicroservice.EventBusRabbitMQ.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FirstMicroservice.EventBusRabbitMQ.EventHandlers
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
            _logger.LogInformation("Received {Event} event on {AppName} app: ({IntegrationEvent})", @event.GetType().Name, "FirstMicroservice", JsonConvert.SerializeObject(@event));

            await Task.CompletedTask;
        }
    }
}