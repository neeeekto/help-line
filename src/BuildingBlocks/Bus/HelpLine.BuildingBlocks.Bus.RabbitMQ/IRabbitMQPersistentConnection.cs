using System;
using RabbitMQ.Client;

namespace HelpLine.BuildingBlocks.Bus.RabbitMQ
{
    internal interface IRabbitMqPersistentConnection
    {
        bool IsConnected { get; }

        bool TryConnect();
        IModel CreateModel();
    }
}
