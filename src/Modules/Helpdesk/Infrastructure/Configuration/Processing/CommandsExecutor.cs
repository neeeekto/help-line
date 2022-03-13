using System.Threading.Tasks;
using Autofac;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing
{
    internal static class CommandsExecutor
    {
        internal static async Task Execute(ICommand command)
        {
            using (var scope = HelpdeskCompositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();
                await mediator.Send(command);
            }
        }

        internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            using (var scope = HelpdeskCompositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();
                return await mediator.Send(command);
            }
        }
    }
}
