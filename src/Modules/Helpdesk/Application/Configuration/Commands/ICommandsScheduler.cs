using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Configuration.Commands
{
    public interface ICommandsScheduler
    {
        /// <summary>
        /// Run command in async queue
        /// </summary>
        /// <param name="command"></param>
        /// <param name="priority"> 0 - low, 8 - height</param>
        /// <returns></returns>
        Task EnqueueAsync(ICommand command, byte priority = 4);

        Task EnqueueAsync<T>(ICommand<T> command, byte priority = 4);
    }
}
