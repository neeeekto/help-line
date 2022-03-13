using System;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;

namespace HelpLine.BuildingBlocks.Infrastructure.EventSourcing
{
    public class AggregateStateSnapshot<TId, TState>
        where TState : IEventsSourcingAggregateState
    {
        public Guid Id { get; }
        public DateTime CreateDate { get; }
        public TId AggregateId { get; }
        public TState State { get; }
        public int Version { get; }

        public AggregateStateSnapshot(TId aggregateId, int version, TState state)
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.UtcNow;
            AggregateId = aggregateId;
            State = state;
            Version = version;
        }
    }
}
