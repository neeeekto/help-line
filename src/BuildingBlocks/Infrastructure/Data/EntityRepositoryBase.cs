using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using MongoDB.Driver;

namespace HelpLine.BuildingBlocks.Infrastructure.Data
{
    public abstract class EntityRepositoryBase<T> : IRepository<T> where T : Entity
    {
        private readonly IMongoContext _context;
        private readonly IDomainEventCollector _eventCollector;

        protected EntityRepositoryBase(IMongoContext context, IDomainEventCollector eventCollector)
        {
            _context = context;
            _eventCollector = eventCollector;
        }


        public async Task Add(T entity)
        {
            CollectEvent(entity);
            await Collection.InsertOneAsync(_context.Session, entity);
        }


        public async Task Update(T entity, bool upsert = false)
        {
            CollectEvent(entity);
            await Collection.ReplaceOneAsync(_context.Session, GetIdFilter(entity), entity,
                new ReplaceOptions() {IsUpsert = upsert});
        }

        public Task Update(T entity) => Update(entity, false);

        public async Task Update(params T[] entities)
        {
            foreach (var entity in entities)
                CollectEvent(entity);
            var modes = entities.Select(x => new ReplaceOneModel<T>(
                    new ExpressionFilterDefinition<T>(GetIdFilter(x)),
                    x
                )
            );
            await Collection.BulkWriteAsync(_context.Session, modes);
        }


        public async Task Remove(T entity)
        {
            CollectEvent(entity);
            await Collection.DeleteOneAsync(_context.Session, GetIdFilter(entity));
        }

        public async Task Remove(Expression<Func<T, bool>> checker)
        {
            await Collection.DeleteManyAsync(_context.Session, checker);
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> checker)
        {
            if (_context.Session != null)
            {
                return await Collection.Find(_context.Session, checker).ToListAsync();
            }
            return await Collection.Find(checker).ToListAsync();
        }

        public Task<T> FindOne(Expression<Func<T, bool>> checker)
        {
            return Collection.Find(checker).FirstOrDefaultAsync();
        }

        private IMongoCollection<T> Collection => _context.GetCollection<T>();

        private void CollectEvent(T entity)
        {
            _eventCollector.Add(entity);
        }

        protected abstract Expression<Func<T, bool>> GetIdFilter(T entity);
    }
}
