using System;

namespace HelpLine.BuildingBlocks.Bus.EventsBus
{
    public interface IEventsBus : IDisposable
    {
        void Publish<T>(T evt) where T : IntegrationEvent;

        void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent;

        void StartConsuming();
    }
}
