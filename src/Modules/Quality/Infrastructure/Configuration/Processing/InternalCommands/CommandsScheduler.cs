using System;
using System.Threading.Tasks;
using HelpLine.Modules.Quality.Application.Configuration.Commands;
using HelpLine.Modules.Quality.Application.Contracts;
using HelpLine.BuildingBlocks.Infrastructure.InternalCommands;

namespace HelpLine.Modules.Quality.Infrastructure.Configuration.Processing.InternalCommands
{
    public class CommandsScheduler : ICommandsScheduler
    {
        private readonly IInternalCommandsQueue _queue;

        public CommandsScheduler(IInternalCommandsQueue queue)
        {
            _queue = queue;
        }


        public Task EnqueueAsync(ICommand command, byte priority = 4)
        {
            _queue.Add(command.Id, command, priority);
            return Task.CompletedTask;
        }

        public Task EnqueueAsync<T>(ICommand<T> command, byte priority = 4)
        {
            _queue.Add(command.Id, command, priority);
            return Task.CompletedTask;
        }
    }
}
