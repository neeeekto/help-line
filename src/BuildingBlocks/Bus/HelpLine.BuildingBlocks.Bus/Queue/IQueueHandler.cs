using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Bus.Queue
{
    public interface IQueueHandler
    {
        public Task TryHandle<T>(T msg);
    }
    public interface IQueueHandler<in T> : IQueueHandler
    {
        public Task Handle(T msg);
    }


}
