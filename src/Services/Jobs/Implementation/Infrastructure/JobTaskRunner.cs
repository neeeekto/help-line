using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.Services.Jobs.Application.Contracts;
using HelpLine.Services.Jobs.Contracts;

namespace HelpLine.Services.Jobs.Infrastructure
{
    internal class JobTaskRunner : IJobTaskRunner
    {
        private readonly IQueue _queue;

        public JobTaskRunner(IQueue queue)
        {
            _queue = queue;
        }

        public Task RunAsync(JobTask task) => _queue.Add(task);
        public void Dispose() => _queue.Dispose();
    }

}
