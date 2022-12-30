using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.BuildingBlocks.Bus.Queue;

namespace HelpLine.BuildingBlocks.IntegrationTests
{
    public class InMemoryEventBus : IEventsBus
    {
        public List<object> Queue { get; }
        public Dictionary<Type, List<IIntegrationEventHandler>> Handlers { get; }

        public InMemoryEventBus()
        {
            Queue = new List<object>();
            Handlers = new Dictionary<Type, List<IIntegrationEventHandler>>();
        }
        public void Dispose()
        {
        }

        public async Task Publish<T>(T evt) where T : IntegrationEvent
        {
            Queue.Add(evt);
        }

        public async Task Emit()
        {
            foreach (var message in Queue)
                if (Handlers.TryGetValue(message.GetType(), out var handlers))
                    foreach (var queueHandler in handlers)
                        await queueHandler.TryHandle(message);
            Queue.Clear();
        }

        public void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
        {
            if (Handlers.TryGetValue(typeof(T), out var handlers))
            {
                handlers.Add(handler);
            }
            else
            {
                Handlers.Add(typeof(T), new List<IIntegrationEventHandler>() {handler});
            }
        }

        public void StartConsuming()
        {
        }
    }
}
