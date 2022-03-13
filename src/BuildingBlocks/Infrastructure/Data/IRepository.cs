using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Infrastructure.Data
{
    public interface IRepository<T>
    {
        Task Add(T entity);
        Task Update(T entity, bool upsert = false);
        Task Update(T entity);
        Task Update(params T[] entities);
        Task Remove(T entity);
        Task Remove(Expression<Func<T, bool>> checker);

        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> checker);
        Task<T> FindOne(Expression<Func<T, bool>> checker);
    }
}
