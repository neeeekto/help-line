using System.Threading.Tasks;
using Autofac;
using HelpLine.Modules.Quality.Application.Contracts;
using MediatR;

namespace HelpLine.Modules.Quality.Infrastructure.Configuration.Processing
{
    internal static class CommandsExecutor
    {
        internal static async Task Execute(ICommand command)
        {
            using (var scope = QualityCompositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();
                await mediator.Send(command);
            }
        }

        internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            using (var scope = QualityCompositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();
                return await mediator.Send(command);
            }
        }
    }
}