using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HelpLine.BuildingBlocks.Infrastructure.Data
{
    public abstract class MongoContext : IMongoContext
    {
        public IMongoClient Client { get; private set; }
        public IClientSessionHandle Session { get; private set; }
        public IMongoDatabase Database { get; private set; }

        public ICollectionNameProvider NameProvider { get; private set; }

        public MongoContext(string connectionStr, string dbName, ICollectionNameProvider nameProvider)
        {
            Client = new MongoClient(connectionStr);
            Database = Client.GetDatabase(dbName);
            NameProvider = nameProvider;
        }

        async Task IMongoContext.BeginTransactionAsync(CancellationToken cancellationToken)
        {
            Session = await Client.StartSessionAsync(cancellationToken: cancellationToken);
            Session.StartTransaction();
        }

        async Task IMongoContext.CommitTransactionAsync(CancellationToken cancellationToken)
        {
            await Session.CommitTransactionAsync(cancellationToken);
            Session?.Dispose();
        }

        async Task IMongoContext.AbortTransactionAsync(CancellationToken cancellationToken)
        {
            await Session.AbortTransactionAsync(cancellationToken);
            Session?.Dispose();
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return Database.GetCollection<T>(NameProvider.Get<T>());
        }

        public void Dispose()
        {
            Session?.Dispose();
        }
    }
}
