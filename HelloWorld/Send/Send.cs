using EventBus.RabbitMQ;
using System;

namespace Sender
{
    class Program
    {
        public static IEventBusRabbitMQ EventBus { get; set; }

        public static void Main(string[] args)
        {
            EventBus = new EventBusRabbitMQ(exchange: "");
            var queueName = "Hi";

            EventBus.Model.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            for (int i = 1; i < 11; i++)
            {
                var message = $"{i}, Hi Benom!";
                EventBus.Publish(message, queueName);
                Console.WriteLine("Sent: {0}", message);
            }

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
