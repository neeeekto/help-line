using System.Threading.Tasks;

namespace HelpLine.Modules.Helpdesk.Application.Contracts
{
    public interface IHelpdeskModule
    {
        Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

        Task ExecuteCommandAsync(ICommand command);

        Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);

    }
}
