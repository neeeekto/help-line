using System.Threading.Tasks;
using Autofac;
using HelpLine.Services.Jobs.Contracts;
using MediatR;

namespace HelpLine.Services.Jobs
{
    public sealed class JobsService
    {
        public Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            return InternalExecuteAsync(command);
        }

        internal static async Task<TResult> InternalExecuteAsync<TResult>(IRequest<TResult> command)
        {
            await using var scope = JobsStartup.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }
}
