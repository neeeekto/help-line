using System;
using HelpLine.BuildingBlocks.Bus.Queue;

namespace HelpLine.BuildingBlocks.Infrastructure.InternalCommands
{
    public interface IInternalCommandsQueue
    {
        void Add<T>(Guid id, T command, byte priority);
        void StartConsuming<T>(InternalCommandTaskHandlerBase<T> handler) where T : InternalCommandTaskBase;
    }
}
