using System.Threading.Tasks;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.BuildingBlocks.Infrastructure.InternalCommands;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing.InternalCommands
{
    public class CommandsScheduler : ICommandsScheduler
    {
        private readonly IInternalCommandsQueue _queue;

        public CommandsScheduler(IInternalCommandsQueue queue)
        {
            _queue = queue;
        }


        public Task EnqueueAsync(ICommand command)
        {
            _queue.Add(command.Id, command);
            return Task.CompletedTask;
        }

        public Task EnqueueAsync<T>(ICommand<T> command)
        {
            _queue.Add(command.Id, command);
            return Task.CompletedTask;
        }

    }
}
