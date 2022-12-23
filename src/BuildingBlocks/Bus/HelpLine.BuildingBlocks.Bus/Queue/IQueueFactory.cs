namespace HelpLine.BuildingBlocks.Bus.Queue
{
    public interface IQueueFactory
    {
        public IQueue MakeQueue(string queueName);
    }
}
