using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Bus.EventsBus;

namespace HelpLine.Apps.Client.API.SignalR
{
    internal class AppBusEventsHandler<T> : IIntegrationEventHandler<T> where T : IntegrationEvent
    {
        private readonly EventToSignalREventsMapper _eventToSignalREventsMapper;

        public AppBusEventsHandler(EventToSignalREventsMapper eventToSignalREventsMapper)
        {
            _eventToSignalREventsMapper = eventToSignalREventsMapper;
        }

        public async Task Handle(T @event)
        {
            await _eventToSignalREventsMapper.HandleEvent(@event);
        }
        public Task TryHandle<T1>(T1 msg)
        {
            if (msg is T @evt)
                return Handle(@evt);
            return Task.CompletedTask;
        }
    }
}
