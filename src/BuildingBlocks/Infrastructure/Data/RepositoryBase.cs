using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace HelpLine.BuildingBlocks.Infrastructure.Data
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        private readonly IMongoContext _context;

        protected RepositoryBase(IMongoContext context)
        {
            _context = context;
        }


        public async Task Add(T entity)
        {
            await Collection.InsertOneAsync(_context.Session, entity);
        }

        public async Task Update(T entity, bool upsert = false)
        {
            await Collection.ReplaceOneAsync(_context.Session, GetIdFilter(entity), entity,
                new ReplaceOptions() {IsUpsert = upsert});
        }

        public Task Update(T entity) => Update(entity, false);

        public async Task Update(params T[] entities)
        {
            var modes = entities.Select(x => new ReplaceOneModel<T>(
                    new ExpressionFilterDefinition<T>(GetIdFilter(x)),
                    x
                )
            );
            await Collection.BulkWriteAsync(_context.Session, modes);
        }


        public async Task Remove(T entity)
        {
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

        public async Task<T> FindOne(Expression<Func<T, bool>> checker)
        {
            if (_context.Session != null)
            {
                return await Collection.Find(_context.Session, checker).FirstOrDefaultAsync();
            }

            return await Collection.Find(checker).FirstOrDefaultAsync();
        }

        protected IMongoCollection<T> Collection => _context.GetCollection<T>();

        protected abstract Expression<Func<T, bool>> GetIdFilter(T entity);
    }
}
