using System.Threading.Tasks;
using Autofac;
using HelpLine.Modules.Quality.Application.Contracts;
using HelpLine.Modules.Quality.Infrastructure.Configuration;
using HelpLine.Modules.Quality.Infrastructure.Configuration.Processing;
using MediatR;

namespace HelpLine.Modules.Quality.Infrastructure
{
    public class QualityModule : IQualityModule
    {
        public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
        {
            return await CommandsExecutor.Execute(command);
        }

        public async Task ExecuteCommandAsync(ICommand command)
        {
            await CommandsExecutor.Execute(command);
        }

        public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            using (var scope = QualityCompositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();

                return await mediator.Send(query);
            }
        }
    }
}
