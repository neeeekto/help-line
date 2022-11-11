using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Infrastructure.InternalCommands
{
    public interface IInternalCommandTaskRepository
    {
        Task InsertAsync(InternalCommandTaskBase task);
    }
}
