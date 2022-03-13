using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Services.Jobs.Infrastructure
{
    internal class JobTaskQueue : IJobTaskQueue
    {
        private readonly IQueue _queue;

        public JobTaskQueue(IQueue queue)
        {
            _queue = queue;
        }

        public void AddHandler<T>(IJobTaskHandler<T> taskHandler) where T : JobTask => _queue.AddHandler(taskHandler);

        public void StartConsuming() => _queue.StartConsuming();
        public void Dispose() => _queue.Dispose();
    }
}
