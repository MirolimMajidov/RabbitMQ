using Common.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Worker
{
    class Program
    {
        public static IEventBusRabbitMQ EventBus { get; set; }

        public static void Main(string[] args)
        {
            EventBus = new EventBusRabbitMQ();
            var queueName = "Hi";
            EventBus.Model.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(EventBus.Model);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine("Received: {0}", message);
            };
            EventBus.Model.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
