using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
{
    class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Hi", durable: false, exclusive: false, autoDelete: false, arguments: null);

                for (int i = 1; i < 11; i++)
                    SendMessage($"{i}, Hi Benom!");

                void SendMessage(string message)
                {
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "", routingKey: "Hi", basicProperties: null, body: body);

                    Console.WriteLine("Sent: {0}", message);
                }
            }

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
