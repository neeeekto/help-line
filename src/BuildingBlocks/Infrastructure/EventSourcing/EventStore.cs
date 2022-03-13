using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.BuildingBlocks.Infrastructure.Data;

namespace HelpLine.BuildingBlocks.Infrastructure.EventSourcing
{
    public abstract class EventStore<TId> : IEventStore<TId>
    {
        private readonly IMongoContext _context;

        protected EventStore(IMongoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventBase<TId>>> Get(TId aggregateId, int fromVersion)
        {
            return await Collection.Find( x => x.AggregateId.Equals(aggregateId))
                .Skip(fromVersion)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventBase<TId>>> Get(TId aggregateId, DateTime atDate)
        {
            return await Collection.Find(
                    x => x.AggregateId.Equals(aggregateId) && x.OccurredOn <= atDate)
                .ToListAsync();
        }

        public Task Add(IEnumerable<EventBase<TId>> events)
        {
            return Collection.InsertManyAsync(_context.Session, events, new InsertManyOptions {IsOrdered = true});
        }

        private IMongoCollection<EventBase<TId>> Collection => _context.GetCollection<EventBase<TId>>();
    }
}
