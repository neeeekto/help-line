namespace HelpLine.BuildingBlocks.Bus.EventsBus
{
    public interface IEventBusFactory
    {
        public IEventsBus MakeEventsBus(string queueName);
    }
}
