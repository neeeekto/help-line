using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Queries;
using HelpLine.BuildingBlocks.Application.Search;
using HelpLine.BuildingBlocks.Application.Search.Contracts;
using MongoDB.Driver;

namespace HelpLine.BuildingBlocks.Infrastructure.Search.Mongo
{
    public class MongoSearchProvider<TModel, TContext> : ISearchProvider<TModel, TContext>
    {
        private readonly IMongoCollection<TModel> _collection;
        private readonly FilterBuilder<TModel> _filterBuilder;
        private readonly SortBuilder<TModel> _sortBuilder;

        public MongoSearchProvider(IFilterValueGetter filterValueGetter, IMongoCollection<TModel> collection,
            IAdditionalTypeProvider typeProvider, IValueMapper valueMapper)
        {
            _collection = collection;
            _filterBuilder = new FilterBuilder<TModel>(filterValueGetter, typeProvider, valueMapper);
            _sortBuilder = new SortBuilder<TModel>();
        }

        public async Task<PagedResult<TModel>> Find(PageData pageData, IFilter filter, TContext context,
            IEnumerable<Sort> sorts)
        {
            var search = _collection
                .Find(GetFilter(filter, context))
                .Sort(GetSort(sorts))
                .Skip(pageData.Skip)
                .Limit(pageData.PerPage);
            var result = await search.ToListAsync();
            var count = await search.CountDocumentsAsync();
            return new PagedResult<TModel>
            {
                Result = result,
                Total = count,
                PageData = pageData
            };
        }

        public async Task<IEnumerable<TModel>> Find(IFilter filter, TContext context, IEnumerable<Sort> sorts)
        {
            var search = _collection
                .Find(GetFilter(filter, context))
                .Sort(GetSort(sorts));
            return await search.ToListAsync();
        }

        public async Task<IEnumerable<TModel>> Find(IFilter? filter, TContext context)
        {
            return await _collection
                .Find(GetFilter(filter, context)).ToListAsync();
        }

        public async Task<IEnumerable<TModel>> Find(TContext context, params IFilter[] filters)
        {
            var builder = new FilterDefinitionBuilder<TModel>();
            var filter = builder.And(filters.Select(x => GetFilter(x, context)));
            return await _collection.Find(filter).ToListAsync();
        }

        public Task<long> GetCount(IFilter filter, TContext context)
        {
            return _collection
                .Find(GetFilter(filter, context)).CountDocumentsAsync();
        }

        // Virtual for cache if application need it!
        protected virtual FilterDefinition<TModel> GetFilter(IFilter? filter, TContext context)
        {
            return _filterBuilder.Build(filter, context);
        }

        // Virtual for cache if application need it!
        protected virtual SortDefinition<TModel> GetSort(IEnumerable<Sort> sorts)
        {
            return _sortBuilder.Build(sorts);
        }
    }
}
