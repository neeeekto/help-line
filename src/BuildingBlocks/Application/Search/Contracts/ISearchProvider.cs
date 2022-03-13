using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Queries;

namespace HelpLine.BuildingBlocks.Application.Search.Contracts
{
    public interface ISearchProvider<TModel, TContext>
    {
        Task<PagedResult<TModel>> Find(PageData pageData, IFilter filter, TContext context, IEnumerable<Sort> sorts);
        Task<IEnumerable<TModel>> Find(IFilter filter, TContext context, IEnumerable<Sort> sorts);
        Task<IEnumerable<TModel>> Find(IFilter filter, TContext context);
        Task<IEnumerable<TModel>> Find(TContext context, params IFilter[] filters);
        Task<long> GetCount(IFilter filter, TContext context);
    }
}
