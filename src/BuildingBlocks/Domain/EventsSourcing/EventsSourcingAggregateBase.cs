using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Domain.EventsSourcing
{
    public abstract class EventsSourcingAggregateBase<TId, TState> : IEventsSourcingAggregateWithState<TId, TState>
        where TState : IEventsSourcingAggregateState
    {
        protected static readonly int InitVersin = 0;

        public TId Id { get; protected set; }
        public TState State { get; protected set; }

        private int _version = InitVersin;

        public int Version => _version;

        private List<EventBase<TId>> _uncommittedEvents = new List<EventBase<TId>>();
        public IReadOnlyCollection<EventBase<TId>> UncommittedEvents => _uncommittedEvents.AsReadOnly();


        protected EventsSourcingAggregateBase(TId id, TState state)
        {
            Id = id;
            State = state;
        }

        protected EventsSourcingAggregateBase(TId id, TState state, int version) : this(id, state)
        {
            _version = version;
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public void ApplyEvent(EventBase<TId> evt)
        {
            ApplyToState(evt);
            _version++;
        }

        protected abstract void ApplyToState(EventBase<TId> evt);

        protected void RiseEvent(EventBase<TId> evt)
        {
            ApplyEvent(evt);
            _uncommittedEvents.Add(evt);
        }

        protected void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }

        protected async Task CheckRule(IBusinessRuleAsync rule)
        {
            if (await rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
    }
}
