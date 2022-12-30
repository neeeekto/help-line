using System;
using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Bus.Queue
{
    public interface IQueue : IDisposable
    {
        void StartConsuming();
        Task Add<T>(T message, byte priority = 0);
        void AddHandler<T>(IQueueHandler<T> handler);
    }
}
