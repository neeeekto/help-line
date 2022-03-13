using System.Threading.Tasks;
using Autofac;
using HelpLine.Services.Migrations.Contracts;
using MediatR;

namespace HelpLine.Services.Migrations
{
    public sealed class MigrationService
    {
        public Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            return InternalExecuteAsync(command);
        }

        internal static async Task<TResult> InternalExecuteAsync<TResult>(IRequest<TResult> command)
        {
            await using var scope = MigrationsStartup.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }
}
