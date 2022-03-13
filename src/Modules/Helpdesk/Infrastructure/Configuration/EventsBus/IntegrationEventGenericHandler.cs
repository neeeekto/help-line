using System.Threading.Tasks;
using Autofac;
using HelpLine.BuildingBlocks.Bus.EventsBus;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.EventsBus
{
    internal class IntegrationEventGenericHandler<T> : IIntegrationEventHandler<T> where T : IntegrationEvent
    {
        public async Task Handle(T @event)
        {
            using var scope = HelpdeskCompositionRoot.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            await mediator.Publish((INotification) @event);
        }
        public Task TryHandle<T1>(T1 msg)
        {
            if (msg is T @evt)
                return Handle(@evt);
            return Task.CompletedTask;
        }
    }
}
