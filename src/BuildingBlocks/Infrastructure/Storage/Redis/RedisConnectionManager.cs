using System;
using StackExchange.Redis;

namespace HelpLine.BuildingBlocks.Infrastructure.Storage.Redis
{
	internal class RedisConnectionManager : IRedisConnectionManager
	{
		private readonly int _databaseNumber;
		public RedisConnectionManager(string connectionString, int databaseNumber)
        {
            _databaseNumber = databaseNumber;
			lock (Locker)
			{
				_lazyConnection ??= new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(
					connectionString));
			}
		}

		private static Lazy<ConnectionMultiplexer>? _lazyConnection;
		private static readonly object Locker = new object();
		private ConnectionMultiplexer Connection => _lazyConnection.Value;
        public IDatabase Db => Connection.GetDatabase(_databaseNumber);
	}
}
