namespace HelpLine.Services.Jobs.Contracts
{
    public interface IJobTaskQueueFactory
    {
        public IJobTaskQueue MakeQueue(string queueName);
    }
}
