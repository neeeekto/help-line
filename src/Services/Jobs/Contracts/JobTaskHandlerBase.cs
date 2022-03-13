using System.Threading.Tasks;

namespace HelpLine.Services.Jobs.Contracts
{
    public abstract class JobTaskHandlerBase<T> : IJobTaskHandler<T> where T : JobTask
    {
        public abstract Task Handle(T task);

        public Task TryHandle<T1>(T1 msg)
        {
            if (msg is T queueMessage)
                return Handle(queueMessage);
            return Task.CompletedTask;
        }
    }
}
