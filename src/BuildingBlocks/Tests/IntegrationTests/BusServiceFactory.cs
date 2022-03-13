using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.BuildingBlocks.IntegrationTests
{
    public class BusServiceFactory : IEventBusFactory, IQueueFactory
    {
        public Dictionary<string, InMemoryEventBus> EventsBuses { get; }
        public Dictionary<string, InMemoryQueue> Queues { get; }

        public BusServiceFactory()
        {
            EventsBuses = new Dictionary<string, InMemoryEventBus>();
            Queues = new Dictionary<string, InMemoryQueue>();
        }

        public IEventsBus MakeEventsBus(string queueName)
        {
            var bus = new InMemoryEventBus();
            EventsBuses.Add(queueName, bus);
            return bus;
        }

        public void PublishInEventBus<T>(T evt) where T : IntegrationEvent
        {
            foreach (var bus in EventsBuses)
            {
                bus.Value.Publish(evt);
            }
        }

        public async Task EmitAllEvents()
        {
            foreach (var eventBus in EventsBuses)
            {
                await eventBus.Value.Emit();
            }
        }

        public async Task EmitAllQueues()
        {
            foreach (var queue in Queues)
            {
                await queue.Value.Emit();
            }
        }

        public async Task EmitAll()
        {
            await EmitAllEvents();
            await EmitAllQueues();
        }

        public async Task PublishInQueues<T>(T evt)
        {
            foreach (var queue in Queues)
            {
                queue.Value.Add(evt);
                await queue.Value.Emit();
            }
        }

        public IQueue MakeQueue(string queueName)
        {
            var queue = new InMemoryQueue();
            Queues.Add(queueName, queue);
            return queue;
        }
    }
}
