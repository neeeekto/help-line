using System;

namespace HelpLine.BuildingBlocks.Bus.Queue
{
    public interface IQueue : IDisposable
    {
        void StartConsuming();
        void Add<T>(T message, byte priority = 0);
        void AddHandler<T>(IQueueHandler<T> handler);
    }
}
