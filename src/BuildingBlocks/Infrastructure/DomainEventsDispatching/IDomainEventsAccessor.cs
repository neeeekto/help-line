using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    public interface IDomainEventsAccessor
    {
        List<IDomainEvent> GetAllDomainEvents();

        void ClearAllDomainEvents();
    }
}