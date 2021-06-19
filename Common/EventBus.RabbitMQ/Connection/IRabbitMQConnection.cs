using RabbitMQ.Client;
using System;

namespace EventBus.RabbitMQ
{
    public interface IRabbitMQConnection : IDisposable
    {
        bool IsConnected { get; }

        int RetryCount { get;  }

        bool TryConnect();

        IModel CreateModel();
    }
}
