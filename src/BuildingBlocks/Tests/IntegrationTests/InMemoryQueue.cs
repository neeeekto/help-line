using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Bus.Queue;
using NUnit.Framework;

namespace HelpLine.BuildingBlocks.IntegrationTests
{
    public class InMemoryQueue : IQueue
    {
        public List<object> Queue { get; }
        public Dictionary<Type, List<IQueueHandler>> Handlers { get; }

        public InMemoryQueue()
        {
            Queue = new List<object>();
            Handlers = new Dictionary<Type, List<IQueueHandler>>();
        }


        public void Dispose()
        {
        }

        public void StartConsuming()
        {
        }

        public void Add<T>(T message)
        {
            Queue.Add(message);
        }

        public void AddHandler<T>(IQueueHandler<T> handler)
        {
            if (Handlers.TryGetValue(typeof(T), out var handlers))
            {
                handlers.Add(handler);
            }
            else
            {
                Handlers.Add(typeof(T), new List<IQueueHandler>() {handler});
            }
        }

        public async Task Emit()
        {
            foreach (var message in Queue.ToList())
                if (Handlers.TryGetValue(message.GetType(), out var handlers))
                    foreach (var queueHandler in handlers)
                        await queueHandler.TryHandle(message);
            Queue.Clear();
        }
    }
}
