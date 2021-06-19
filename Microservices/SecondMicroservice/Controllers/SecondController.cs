using EventBus.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SecondMicroservice.EventBusRabbitMQ.Events;
using System.Text;

namespace SecondMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecondController : ControllerBase
    {
        private readonly ILogger<SecondController> _logger;
        private readonly IEventBusRabbitMQ _eventBus;

        public SecondController(ILogger<SecondController> logger, IEventBusRabbitMQ eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        [HttpGet]
        public bool Get(string message = "Default test SecondMicroservice")
        {
            _logger.LogInformation("Send '{0}' message from SecondMicroservice.", message);
            var test = new FirstTestEvent() { Message = message };
            var test2 = new SecondTestEvent() { Message = message };
            _eventBus.Publish(test);
            _eventBus.Publish(test2);

            return true;
        }
    }
}
