using System.Threading.Tasks;
using MongoDB.Driver;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.BuildingBlocks.Infrastructure.Data;

namespace HelpLine.BuildingBlocks.Infrastructure.EventSourcing
{
    public abstract class ShanpshotStore<TId, TState> : ISnapshotStore<TId, TState>
        where TState : IEventsSourcingAggregateState
    {
        private readonly IMongoContext _context;

        protected ShanpshotStore(IMongoContext context)
        {
            _context = context;
        }

        public async Task Add(TId aggregateId, int version, TState state)
        {
            await Collection.InsertOneAsync(_context.Session,
                new AggregateStateSnapshot<TId, TState>(aggregateId, version, state));
        }

        public Task<AggregateStateSnapshot<TId, TState>> Get(TId aggregateId)
        {
            return Collection
                .Find(_context.Session,x => x.AggregateId.Equals(aggregateId))
                .SortByDescending(x => x.Version)
                .FirstOrDefaultAsync();
        }

        private IMongoCollection<AggregateStateSnapshot<TId, TState>> Collection =>
            _context.GetCollection<AggregateStateSnapshot<TId, TState>>();
    }
}