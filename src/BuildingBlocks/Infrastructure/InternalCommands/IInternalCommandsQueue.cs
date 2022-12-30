using System;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Bus.Queue;

namespace HelpLine.BuildingBlocks.Infrastructure.InternalCommands
{
    public interface IInternalCommandsQueue
    {
        Task Add<T>(Guid id, T command, byte priority);
        void StartConsuming<T>(InternalCommandTaskHandlerBase<T> handler) where T : InternalCommandTaskBase;
    }
}
