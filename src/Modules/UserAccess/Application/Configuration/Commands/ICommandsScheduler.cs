using System.Threading.Tasks;
using HelpLine.Modules.UserAccess.Application.Contracts;

namespace HelpLine.Modules.UserAccess.Application.Configuration.Commands
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync(ICommand command);

        Task EnqueueAsync<T>(ICommand<T> command);
    }
}