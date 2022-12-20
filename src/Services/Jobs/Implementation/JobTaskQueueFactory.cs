using HelpLine.BuildingBlocks.Bus.Queue;
using HelpLine.Services.Jobs.Contracts;
using HelpLine.Services.Jobs.Infrastructure;

namespace HelpLine.Services.Jobs
{
    public sealed class JobTaskQueueFactory : IJobTaskQueueFactory
    {
        private readonly IQueueFactory _queueFactory;

        public JobTaskQueueFactory(IQueueFactory queueFactory)
        {
            _queueFactory = queueFactory;
        }

        public IJobTaskQueue MakeQueue(string queueName)
        {
            return new JobTaskQueue(_queueFactory.MakeQueue(queueName));
        }
    }
}
