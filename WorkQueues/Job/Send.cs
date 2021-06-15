using Common.RabbitMQ;
using System;

namespace Job
{
    class Program
    {
        public static IEventBusRabbitMQ EventBus { get; set; }

        static int JobId = 1;
        public static void Main(string[] args)
        {
            EventBus = new EventBusRabbitMQ(exchange: "");

            var queueName = "job_queue";
            EventBus.Model.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            SendMessage(queueName);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }

        static void SendMessage(string queueName)
        {
            Console.Write($"Enter your {JobId} task:");
            var job = Console.ReadLine();

            var properties = EventBus.Model.CreateBasicProperties();
            properties.Persistent = true;

            EventBus.Publish(job, queueName, properties);
            Console.WriteLine("[x] Sent '{0}':'{1}'", queueName, job);

            JobId++;

            SendMessage(queueName);
        }
    }
}
