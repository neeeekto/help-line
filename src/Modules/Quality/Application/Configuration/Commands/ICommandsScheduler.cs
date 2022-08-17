using System.Threading.Tasks;
using HelpLine.Modules.Quality.Application.Contracts;

namespace HelpLine.Modules.Quality.Application.Configuration.Commands
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync(ICommand command, byte priority = 4);

        Task EnqueueAsync<T>(ICommand<T> command, byte priority = 4);
    }
}
