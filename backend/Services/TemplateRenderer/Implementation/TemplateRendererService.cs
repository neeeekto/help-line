using System.Threading.Tasks;
using Autofac;
using HelpLine.Services.TemplateRenderer.Contracts;
using MediatR;

namespace HelpLine.Services.TemplateRenderer
{
    public sealed class TemplateRendererService
    {
        public Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            return InternalExecuteAsync(command);
        }

        internal static async Task<TResult> InternalExecuteAsync<TResult>(IRequest<TResult> command)
        {
            await using var scope = TemplateRendererStartup.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(command);
        }
    }
}
