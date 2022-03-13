using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Storage;

namespace HelpLine.BuildingBlocks.IntegrationTests
{
    public class InMemoryStorage : IStorage
    {
        private readonly Dictionary<string, object> _storage = new();
        private string ns;

        public InMemoryStorage(string ns)
        {
            this.ns = ns;
        }

        public virtual async Task<T> Get<T>(object id)
        {
            var key = $"{ns}_{id}";
            if (_storage.ContainsKey(key))
                return ((T) _storage[key]);
            return default;
        }

        public async Task<IEnumerable<T>> GetList<T>()
        {
            return _storage.Select(x => (T) x.Value);
        }

        public virtual Task Set<T>(object id, T item, TimeSpan? expiry = null)
        {
            var key = $"{ns}_{id}";
            _storage[key] = item;
            return Task.CompletedTask;
        }

        public virtual Task RemoveOne(object id)
        {
            var key = $"{ns}_{id}";
            _storage.Remove(key);
            return Task.CompletedTask;
        }

        public virtual Task RemoveMany(IEnumerable<object> ids)
        {
            foreach (var id in ids)
            {
                var key = $"{ns}_{id}";
                _storage.Remove(key);
            }

            return Task.CompletedTask;
        }
    }

    public class InMemoryStorage<T> : InMemoryStorage, IStorage<T>
    {
        public InMemoryStorage(string ns) : base(ns)
        {
        }

        public virtual Task<T> Get(object id) => base.Get<T>(id);
        public virtual Task<IEnumerable<T>> GetList() => base.GetList<T>();

        public virtual Task Set(object id, T item, TimeSpan? expiry = null) => base.Set(id, item, expiry);

        public virtual Task RemoveOne(object id) => base.RemoveOne(id);

        public virtual Task RemoveMany(IEnumerable<object> ids)  => base.RemoveMany(ids);
    }
}
