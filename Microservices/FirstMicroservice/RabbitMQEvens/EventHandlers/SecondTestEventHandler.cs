using EventBus.RabbitMQ;
using FirstMicroservice.RabbitMQEvens.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FirstMicroservice.RabbitMQEvens.EventHandlers
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