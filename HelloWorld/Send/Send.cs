using EventBus.RabbitMQ;
using System;

namespace Sender
{
    class Program
    {
        public static IEventBusRabbitMQ EventBus { get; set; }

        static int MessageId = 1;
        public static void Main(string[] args)
        {
            EventBus = new EventBusRabbitMQ(exchange: "");
            var queueName = "Hi";

            EventBus.Model.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            SendMessage(queueName);
            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }

        static void SendMessage(string queueName)
        {
            Console.Write($"Enter your {MessageId} message:");
            var message = Console.ReadLine();

            EventBus.Publish(message, queueName);
            Console.WriteLine("[x] Sent:'{0}'", message);

            MessageId++;

            SendMessage(queueName);
        }
    }
}
