using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}