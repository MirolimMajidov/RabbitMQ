using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ReceiveService
{
    public class Receive
    {
        public static IEventBusRabbitMQ EventBus { get; set; }

        public static void Main()
        {
            EventBus = new EventBusRabbitMQ();
            EventBus.Exchange = "sender";
            EventBus.Model.ExchangeDeclare(exchange: EventBus.Exchange, type: ExchangeType.Direct);

            Console.Write($"Enter Routing Key:");
            var routingKey = Console.ReadLine();

            var queueName = EventBus.Model.QueueDeclare().QueueName;

            EventBus.Model.QueueBind(queue: queueName,
                              exchange: EventBus.Exchange,
                              routingKey: routingKey);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(EventBus.Model);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(" [x] Received '{0}':'{1}'",
                                  routingKey, message);
            };
            EventBus.Model.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
