using StackExchange.Redis;

namespace HelpLine.BuildingBlocks.Infrastructure.Storage.Redis
{
    public interface IRedisConnectionManager
    {
        IDatabase Db { get; }
    }
}
