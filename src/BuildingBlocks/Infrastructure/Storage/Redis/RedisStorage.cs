using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HelpLine.BuildingBlocks.Infrastructure.Storage.Redis
{
    internal class RedisStorage : IStorage
    {
        private readonly IRedisConnectionManager _connectionManager;
        private readonly string _cacheKey;

        public RedisStorage(IRedisConnectionManager connectionManager, string cacheKey)
        {
            _connectionManager = connectionManager;
            _cacheKey = cacheKey;
        }

        public async Task<T> Get<T>(object id)
        {
            var itemJson = await _connectionManager.Db.StringGetAsync(GetKey(id));
            if (!itemJson.HasValue)
                return default;

            var item = JsonConvert.DeserializeObject<T>(itemJson);
            return item;
        }

        public async Task<IEnumerable<T>> GetList<T>()
        {
            var data = new List<RedisValue>();

            //TODO: Need optimisations. A lot of asyncs. wait key -> wait get -> repeat...
            foreach (var endPoint in _connectionManager.Db.Multiplexer.GetEndPoints())
            {
                var server = _connectionManager.Db.Multiplexer.GetServer(endPoint);
                await foreach(var key in server.KeysAsync(pattern: $"*{_cacheKey}*")) {
                    data.Add(await _connectionManager.Db.StringGetAsync(key));
                }
            }


            return data?.Where(x => x.HasValue).Select(x => JsonConvert.DeserializeObject<T>(x)) ?? Array.Empty<T>();
        }

        public Task Set<T>(object id, T item, TimeSpan? expiry = null)
        {
            return _connectionManager.Db.StringSetAsync(GetKey(id), new RedisValue(JsonConvert.SerializeObject(item)),
                expiry);
        }

        public Task RemoveOne(object id)
        {
            return _connectionManager.Db.KeyDeleteAsync(GetKey(id));
        }

        public Task RemoveMany(IEnumerable<object> ids)
        {
            return _connectionManager.Db.KeyDeleteAsync(ids.Select(x => new RedisKey(GetKey(x))).ToArray());
        }

        protected string GetKey(object id) => $"{_cacheKey}:{id}";
    }

    internal class RedisStorage<T> : RedisStorage, IStorage<T>
    {
        public RedisStorage(IRedisConnectionManager connectionManager, string cacheKey) : base(connectionManager,
            cacheKey)
        {
        }

        public Task<T> Get(object id) => base.Get<T>(id);
        public Task<IEnumerable<T>> GetList() => base.GetList<T>();

        public Task Set(object id, T item, TimeSpan? expiry = null) => base.Set(id, item, expiry);
    }
}
