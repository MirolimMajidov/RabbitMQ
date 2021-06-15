using Common.RabbitMQ;
using RabbitMQ.Client;
using System;

namespace PublisherService
{
    public class Send
    {
        static int MessageId = 1;
        public static IEventBusRabbitMQ EventBus { get; set; }

        public static void Main(string[] args)
        {
            EventBus = new EventBusRabbitMQ();
            EventBus.Model.ExchangeDeclare(exchange: EventBus.Exchange, type: ExchangeType.Fanout);

            SendMessage();

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        static void SendMessage()
        {
            Console.Write($"Enter your {MessageId} message:");
            var message = Console.ReadLine();

            EventBus.Publish(message, routingKey: "");
            Console.WriteLine("[x] Sent:'{0}'", message);

            MessageId++;

            SendMessage();
        }
    }
}
