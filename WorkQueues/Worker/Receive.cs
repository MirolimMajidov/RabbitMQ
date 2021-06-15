using EventBus.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Worker
{
    class Program
    {
        public static IEventBusRabbitMQ EventBus { get; set; }
        
        public static void Main(string[] args)
        {
            EventBus = new EventBusRabbitMQ();
            var queueName = "job_queue";
            EventBus.Model.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            EventBus.Model.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            Console.WriteLine("[*] Waiting for tasks.");

            var consumer = new EventingBasicConsumer(EventBus.Model);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("[x] Received '{0}' task", message);

                int secunds = new Random().Next(2, 5);
                Thread.Sleep(secunds * 1000);

                Console.WriteLine("[x] '{0}' task done", message);

                EventBus.Model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            EventBus.Model.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
