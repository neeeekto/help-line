using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    public interface IDomainEventCollector
    {
        void Add(Entity entity);
    }
}