using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiveService
{
    public interface IEventBusRabbitMQ
    {
        IModel Model { get; set; }

        string Exchange { get; set; }

        void Publish(string message);
    }
}
