using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching;

namespace HelpLine.BuildingBlocks.Infrastructure.EventSourcing
{
    public abstract class
        EventsSourcingAggregateRepository<TId, TAggregate, TState> : IEventsSourcingAggregateRepository<TId, TAggregate>
        where TState : IEventsSourcingAggregateState
        where TAggregate : class, IEventsSourcingAggregateWithState<TId, TState>
    {
        private readonly int _snapshotPeriod;

        private readonly IEventStore<TId> _eventStore;
        private readonly ISnapshotStore<TId, TState> _snapshotStore;
        private readonly IDomainEventCollector _eventCollector;

        protected EventsSourcingAggregateRepository(int snapshotPeriod, IEventStore<TId> eventStore,
            ISnapshotStore<TId, TState> snapshotStore, IDomainEventCollector eventCollector)
        {
            _snapshotPeriod = snapshotPeriod;
            _eventStore = eventStore;
            _snapshotStore = snapshotStore;
            _eventCollector = eventCollector;
        }

        public async Task SaveAsync(TAggregate aggregate,
            CancellationToken cancellationToken = default)
        {
            var eventsToSave = aggregate.UncommittedEvents.ToList();
            var expectedVersion = CalculateExpectedVersion(aggregate, eventsToSave);

            if (expectedVersion > 0)
            {
                var existingEvents = await _eventStore.Get(aggregate.Id, 0);
                var currentversion = existingEvents.Count();
                if (currentversion != expectedVersion)
                {
                    throw new WrongExpectedVersionException(
                        $"{aggregate.GetType()}:{aggregate.Id}: Expected version {expectedVersion} but the version is {currentversion}");
                }
            }

            if (((aggregate.Version + 1) % _snapshotPeriod) == 0)
            {
                await _snapshotStore.Add(aggregate.Id, aggregate.Version, aggregate.State);
            }

            if (eventsToSave.Any())
            {
                await _eventStore.Add(eventsToSave);
                _eventCollector.Add(new EsEventProvider(aggregate));
            }

        }

        public async Task<TAggregate> GetByIdAsync(TId id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var lastSnapshot = await _snapshotStore.Get(id);
            TAggregate aggregate;

            if (lastSnapshot == null)
                aggregate = (TAggregate) Activator.CreateInstance(typeof(TAggregate),
                    BindingFlags.Instance | BindingFlags.NonPublic, null,
                    new object[] {id}, null, null);
            else
                aggregate = (TAggregate) Activator.CreateInstance(typeof(TAggregate),
                    BindingFlags.Instance | BindingFlags.NonPublic, null,
                    new object[] {id, lastSnapshot.State, lastSnapshot.Version}, null, null);
            if (aggregate == null)
                throw new ApplicationException($"Failed to create instance of {typeof(TAggregate).FullName} type");

            var events = await _eventStore.Get(id, lastSnapshot?.Version ?? 0);
            if (!events.Any())
            {
                return null;
            }

            foreach (var evt in events)
            {
                aggregate.ApplyEvent(evt);
            }

            return aggregate;
        }

        private int CalculateExpectedVersion(TAggregate aggregate, IEnumerable<EventBase<TId>> events) =>
            aggregate.Version - events.Count();

        // Грязный хак)))
        private class EsEventProvider : Entity
        {
            private readonly TAggregate _aggregate;

            internal EsEventProvider(TAggregate aggregate)
            {
                _aggregate = aggregate;
                foreach (var evt in aggregate.UncommittedEvents)
                {
                    AddDomainEvent(evt);
                }
            }


            public override void ClearDomainEvents()
            {
                base.ClearDomainEvents();
                _aggregate.ClearUncommittedEvents();
            }
        }
    }
}
