using EventBus.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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
            //_logger.LogInformation("Send '{0}' message.", message);

            //var queueName = _eventBus.Model.QueueDeclare().QueueName;
            //_eventBus.Model.QueueBind(queue: queueName, exchange: _eventBus.Exchange, routingKey: routingKey);

            //_logger.LogInformation("[*] Waiting for messages.");

            //var consumer = new EventingBasicConsumer(_eventBus.Model);
            //consumer.Received += (model, ea) =>
            //{
            //    var body = ea.Body.ToArray();
            //    var message = Encoding.UTF8.GetString(body);

            //    _logger.LogInformation("[x] Received '{0}':'{1}'", routingKey, message);
            //};
            //_eventBus.Model.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            return true;
        }
    }
}
