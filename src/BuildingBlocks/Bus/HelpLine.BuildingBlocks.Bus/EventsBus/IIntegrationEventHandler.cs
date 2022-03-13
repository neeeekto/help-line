using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Bus.EventsBus
{
    public interface IIntegrationEventHandler
    {
        public Task TryHandle<T>(T msg);
    }

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent: IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }


}
