using System.Threading.Tasks;

namespace HelpLine.Modules.UserAccess.Application.Contracts
{
    public interface IUserAccessModule
    {
        Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

        Task ExecuteCommandAsync(ICommand command);

        Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
    }
}