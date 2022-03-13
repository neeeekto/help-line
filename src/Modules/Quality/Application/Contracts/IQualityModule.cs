using System.Threading.Tasks;

namespace HelpLine.Modules.Quality.Application.Contracts
{
    public interface IQualityModule
    {
        Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

        Task ExecuteCommandAsync(ICommand command);

        Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
    }
}
