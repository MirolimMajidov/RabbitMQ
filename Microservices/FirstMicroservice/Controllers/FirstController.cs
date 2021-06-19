using EventBus.RabbitMQ;
using FirstMicroservice.EventBusRabbitMQ.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FirstMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FirstController : ControllerBase
    {
        private readonly ILogger<FirstController> _logger;
        private readonly IEventBusRabbitMQ _eventBus;

        public FirstController(ILogger<FirstController> logger, IEventBusRabbitMQ eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        [HttpGet]
        public bool Get(string message = "Default test FirstMicroservice")
        {
            _logger.LogInformation("Send '{0}' message from FirstMicroservice.", message);
            var test = new FirstTestEvent() { Message = message };
            _eventBus.Publish(test);

            return true;
        }
    }
}
