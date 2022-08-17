using System.Threading.Tasks;
using Autofac;
using HelpLine.Services.Files.Contracts;
using MediatR;

namespace HelpLine.Services.Files
{
    public sealed class FilesService
    {
        public Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            return InternalExecuteAsync(command);
        }

        internal static async Task<TResult> InternalExecuteAsync<TResult>(IRequest<TResult> command)
        {
            await using var scope = FilesStartup.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }
}
