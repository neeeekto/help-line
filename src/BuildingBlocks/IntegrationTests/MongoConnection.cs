using MongoDB.Driver;

namespace HelpLine.BuildingBlocks.IntegrationTests
{
    public class MongoConnection
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }

        public MongoConnection(string connStr, string dbName)
        {
            Client = new MongoClient(connStr);
            Database = Client.GetDatabase(dbName);
        }
    }
}
