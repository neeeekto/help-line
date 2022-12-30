using System;
using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Bus.EventsBus
{
    public interface IEventsBus : IDisposable
    {
        Task Publish<T>(T evt) where T : IntegrationEvent;

        void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent;

        void StartConsuming();
    }
}
