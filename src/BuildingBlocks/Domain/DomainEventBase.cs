using System;

namespace HelpLine.BuildingBlocks.Domain
{
    public class DomainEventBase : IDomainEvent
    {
        public DateTime OccurredOn { get; private set; }

        public DomainEventBase()
        {
            OccurredOn = DateTime.UtcNow;
        }
    }
}
