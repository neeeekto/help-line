using System.Collections.Generic;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using NSubstitute;
using NSubstitute.ClearExtensions;

namespace HelpLine.BuildingBlocks.IntegrationTests
{
    public class InMemoryStorageFactory : IStorageFactory
    {
        public Dictionary<string, InMemoryStorage> CacheStorages = new ();
        public IStorage Make(string cacheKey)
        {
            var storage = Substitute.ForPartsOf<InMemoryStorage>(cacheKey);
            CacheStorages.Add(cacheKey, storage);
            return storage;
        }

        public IStorage<T> Make<T>(string cacheKey)
        {
            var storage = Substitute.ForPartsOf<InMemoryStorage<T>>(cacheKey);
            CacheStorages.Add(cacheKey, storage);
            return storage;
        }

        public void Clear()
        {
            foreach (var inMemoryCacheStorage in CacheStorages)
            {
                inMemoryCacheStorage.Value.ClearSubstitute();
            }
        }
    }
}
