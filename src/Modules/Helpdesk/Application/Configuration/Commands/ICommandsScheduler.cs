using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Configuration.Commands
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync(ICommand command);

        Task EnqueueAsync<T>(ICommand<T> command);
    }
}
