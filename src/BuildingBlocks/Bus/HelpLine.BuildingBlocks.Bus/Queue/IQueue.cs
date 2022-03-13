using System;

namespace HelpLine.BuildingBlocks.Bus.Queue
{
    public interface IQueue : IDisposable
    {
        void StartConsuming();
        void Add<T>(T message);
        void AddHandler<T>(IQueueHandler<T> handler);
    }
}
