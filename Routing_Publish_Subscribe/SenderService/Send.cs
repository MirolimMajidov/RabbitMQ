using Common.RabbitMQ;
using RabbitMQ.Client;
using System;

namespace SenderService
{
    public class Send
    {
        static int MessageId = 1;
        public static IEventBusRabbitMQ EventBus { get; set; }

        public static void Main(string[] args)
        {
            EventBus = new EventBusRabbitMQ(exchange: "MicroService");
            EventBus.Model.ExchangeDeclare(exchange: EventBus.Exchange, type: ExchangeType.Direct);

            SendMessage();

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        static void SendMessage()
        {
            Console.Write($"Enter your {MessageId} message:");
            var message = Console.ReadLine();

            Console.Write($"Enter server name:");
            var server = Console.ReadLine();

            EventBus.Publish(message, server);
            Console.WriteLine("[x] Sent '{0}':'{1}'", server, message);

            MessageId++;

            SendMessage();
        }
    }
}
