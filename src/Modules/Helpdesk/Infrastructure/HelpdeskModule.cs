using System.Threading.Tasks;
using Autofac;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration;
using HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Infrastructure
{
    public class HelpdeskModule : IHelpdeskModule
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
            await using var scope = HelpdeskCompositionRoot.BeginLifetimeScope();
            var mediator = scope.Resolve<IMediator>();
            return await mediator.Send(query);
        }
    }
}
