using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Domain.EventsSourcing
{
    public interface IEventsSourcingAggregate<TId>
    {
        TId Id { get; }
        int Version { get; }
        IReadOnlyCollection<EventBase<TId>> UncommittedEvents { get; }
        void ClearUncommittedEvents();
        void ApplyEvent(EventBase<TId> evt);
    }

    public interface IEventsSourcingAggregateWithState<TId, out TState> : IEventsSourcingAggregate<TId>
        where TState : IEventsSourcingAggregateState
    {
        TState State { get; }
    }
}
