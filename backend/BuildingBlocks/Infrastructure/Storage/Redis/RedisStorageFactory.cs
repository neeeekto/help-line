namespace HelpLine.BuildingBlocks.Infrastructure.Storage.Redis
{
    public class RedisStorageFactory : IStorageFactory
    {
        private readonly IRedisConnectionManager _redisConnection;

        public RedisStorageFactory(string connectionString, int databaseNumber)
        {
            _redisConnection = new RedisConnectionManager(connectionString, databaseNumber);
        }

        public IStorage Make(string cacheKey)
        {
            return new RedisStorage(_redisConnection, cacheKey);
        }

        public IStorage<T> Make<T>(string cacheKey)
        {
            return new RedisStorage<T>(_redisConnection, cacheKey);
        }
    }
}
