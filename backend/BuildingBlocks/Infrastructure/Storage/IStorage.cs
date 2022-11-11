using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Infrastructure.Storage
{
    public interface IStorage
    {
        Task<T> Get<T>(object id);
        Task<IEnumerable<T>> GetList<T>();
        Task Set<T>(object id, T item, TimeSpan? expiry = null);
        Task RemoveOne(object id);
        Task RemoveMany(IEnumerable<object> ids);
    }

    public interface IStorage<T>
    {
        Task<T> Get(object id);
        Task<IEnumerable<T>> GetList();
        Task Set(object id, T item, TimeSpan? expiry = null);
        Task RemoveOne(object id);
        Task RemoveMany(IEnumerable<object> ids);
    }
}
