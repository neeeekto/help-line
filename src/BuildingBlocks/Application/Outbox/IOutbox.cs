using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Application.Outbox
{
    public interface IOutbox
    {
        Task Add(OutboxMessage message);
        Task Publish();
    }
}
