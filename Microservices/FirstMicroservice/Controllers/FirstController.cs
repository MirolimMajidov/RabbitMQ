using EventBus.RabbitMQ;
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
            _logger.LogInformation("Send '{0}' message.", message);
            //_eventBus.Publish(message, "SecondMicroservice");

            return true;
        }
    }
}
