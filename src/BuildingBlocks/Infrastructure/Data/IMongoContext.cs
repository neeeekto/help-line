using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace HelpLine.BuildingBlocks.Infrastructure.Data
{
    public interface IMongoContext : IDisposable
    {
        public IMongoClient Client { get; }
        public IClientSessionHandle Session { get; }
        public IMongoDatabase Database { get; }
        public ICollectionNameProvider NameProvider { get; }

        internal Task BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));
        internal Task CommitTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));
        internal Task AbortTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));

        public IMongoCollection<T> GetCollection<T>();
    }
}
