using System;
using MediatR;

namespace HelpLine.BuildingBlocks.Bus.EventsBus
{
    public abstract class IntegrationEvent : INotification
    {
        public Guid Id { get; }

        public DateTime OccurredOn { get; }

        protected IntegrationEvent(Guid id, DateTime occurredOn)
        {
            Id = id;
            OccurredOn = occurredOn;
        }
    }
}
